using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using aeternaCaptions.src.model;

namespace aeternaCaptions.src.Utils
{
    public static class MediaProcessor
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly IConfigurationSection AssemblyAIConfig;

        static MediaProcessor()
        {
            if (StaticConfiguration.config == null)
            {
                throw new InvalidOperationException("Configuration not initialized. Call StaticConfiguration.Initialize() before accessing configuration settings.");
            }

            AssemblyAIConfig = StaticConfiguration.config.GetSection("AssemblyAI");



            // Set the base address for AssemblyAi
            httpClient.BaseAddress = new Uri(AssemblyAIConfig.GetValue<string>("BaseUri")!);


            // Set the Authorization header for the httpClient instance
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(StaticConfiguration.AssemblyAiApiKey!);
        }

        public static async Task<Transcript?> GenerateCaptionsAsync(string videoPath, string videoTitle)
        {
            string? tempVideoPath = null;
            string? tempAudioPath = null;
            Transcript? transcript = null;
            try
            {
                tempVideoPath = await UploadVideoAsync(videoPath);
                tempAudioPath = await ExtractAudioFromVideoAsync(tempVideoPath);
                string uploadUrl = await UploadFileToTranscriptAsync(tempAudioPath);
                transcript = await CreateTranscriptAsync(uploadUrl);
                if (transcript != null)
                {
                    transcript = await GetTranscriptFromAssemblyAIServerAsync(transcript.Id);
                    if (transcript != null)
                        transcript.Title = videoTitle;
                }
            }
            finally
            {
                DeleteTempFiles([tempVideoPath!, tempAudioPath!]);
            }
            return transcript;
        }

        public static async Task<string> ExportSubtitlesAsync(string transcriptId, string format)
        {
            var uriBuilder = new UriBuilder(httpClient.BaseAddress!);
            uriBuilder.Path += AssemblyAIConfig.GetValue<string>("TranscriptUri")!;
            uriBuilder.Path += $"/{transcriptId}/{format}";
            var finalUri = uriBuilder.Uri;

            using(HttpResponseMessage response = await httpClient.GetAsync(finalUri))
            {
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
            }
        }


        private static async Task<string> UploadVideoAsync(string videoPath)
        {
            if (IsUrl(videoPath))
            {
                return await DownloadVideoFromUrlAsync(videoPath);
            }
            else if (File.Exists(videoPath))
            {
                return await CopyVideoToTempLocationAsync(videoPath);
            }
            else
            {
                throw new ArgumentException("Invalid video path provided. ");
            }
        }

        private static bool IsUrl(string path)
        {
            Uri? uriResult;
            return Uri.TryCreate(path, UriKind.Absolute, out uriResult) &&
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }


        private static async Task<string> DownloadVideoFromUrlAsync(string videoPath)
        {
            //TODO implement this method if necesary, handle youtube (easy with y.explode) and drive (Requires google api and oauth2)
            return "";
        }

        private static async Task<string> CopyVideoToTempLocationAsync(string videoPath)
        {
            return await Task.Run(() =>
            {
                string tempFilePath = Path.GetTempFileName() + Path.GetExtension(videoPath);
                File.Copy(videoPath, tempFilePath, true);
                return tempFilePath;
            });
        }

        private static async Task<string> ExtractAudioFromVideoAsync(string tempVideoPath)
        {
            string audioPath = Path.GetTempFileName() + ".wav"; //added for ffmpegArgs sake
            string ffmpegArgs = $"-i \"{tempVideoPath}\" \"{audioPath}\"";



            // Start FFmpeg process
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegArgs,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process ffmpegProcess = new Process
            {
                StartInfo = psi
            };

            ffmpegProcess.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data); //TODO LOG PROPERLY
            ffmpegProcess.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

            ffmpegProcess.Start();
            ffmpegProcess.BeginOutputReadLine();
            ffmpegProcess.BeginErrorReadLine();

            await ffmpegProcess.WaitForExitAsync();

            return audioPath;
        }

        private static async Task<string> UploadFileToTranscriptAsync(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            using (var fileContent = new StreamContent(fileStream))
            {
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var uriBuilder = new UriBuilder(httpClient.BaseAddress!);
                uriBuilder.Path += AssemblyAIConfig.GetValue<string>("UploadUri")!;
                var finalUri = uriBuilder.Uri;

                using (var response = await httpClient.PostAsync(finalUri, fileContent))
                {
                    response.EnsureSuccessStatusCode();
                    var jsonDoc = await response.Content.ReadFromJsonAsync<JsonDocument>();
                    return jsonDoc!.RootElement.GetProperty("upload_url").GetString()!;
                }
            }
        }

        private static async Task<Transcript?> CreateTranscriptAsync(string audioUploadUrl)
        {
            var data = new { audio_url = audioUploadUrl, language_detection = true };
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            var uriBuilder = new UriBuilder(httpClient.BaseAddress!);
            uriBuilder.Path += AssemblyAIConfig.GetValue<string>("TranscriptUri")!;
            var finalUri = uriBuilder.Uri;

            using (var response = await httpClient.PostAsync(finalUri, content))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Transcript>();
            }
        }

        public static async Task<Transcript?> GetTranscriptFromAssemblyAIServerAsync(string transcriptId)
        {
            var uriBuilder = new UriBuilder(httpClient.BaseAddress!);
            uriBuilder.Path += AssemblyAIConfig.GetValue<string>("TranscriptUri")!;
            uriBuilder.Path += $"/{transcriptId}";
            string pollingEndpoint = uriBuilder.Uri.ToString();

            while (true)
            {
                var pollingResponse = await httpClient.GetAsync(pollingEndpoint);
                var finishedTranscript = await pollingResponse.Content.ReadFromJsonAsync<Transcript>();
                switch (finishedTranscript!.Status)
                {
                    case "processing":
                    case "queued":
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        break;
                    case "completed":
                        return finishedTranscript;
                    case "error":
                        throw new Exception($"Transcription failed: {finishedTranscript.Error}");
                    default:
                        return null;
                }
            }
        }

        public static async Task<byte[]> BurnSubtitlesToVideo(string videoPath, SubtitleFile subtitle)
        {

            string tempVideoPath = await CopyVideoToTempLocationAsync(videoPath);
            string tempSubtitlePath = Path.GetTempFileName() + ".srt";
            await File.AppendAllTextAsync(tempSubtitlePath, subtitle.SubtitleContent);

            string tempOutputVideoPath = Path.GetTempFileName() + Path.GetExtension(videoPath);

                // Command to burn subtitles onto the video using FFmpeg
            string ffmpegArgs = $"-i \"{tempVideoPath}\" -vf subtitles=\"{tempSubtitlePath}\" \"{tempOutputVideoPath}\"";

            // Start FFmpeg process
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegArgs,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process ffmpegProcess = new Process
            {
                StartInfo = psi
            };

            ffmpegProcess.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data); //TODO LOG PROPERLY
            ffmpegProcess.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

            ffmpegProcess.Start();
            ffmpegProcess.BeginOutputReadLine();
            ffmpegProcess.BeginErrorReadLine();

            await ffmpegProcess.WaitForExitAsync();

            byte[] videoBytes = await File.ReadAllBytesAsync(tempOutputVideoPath);
            DeleteTempFiles([tempVideoPath, tempSubtitlePath, tempOutputVideoPath]);
            return videoBytes;
        }


        private static bool DeleteTempFiles(params string[] filePaths)
        {
            if(filePaths.Any())
            {
                return false;
            }
            
            foreach(string filePath in filePaths)
            {
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            return true;
        }

    }
}