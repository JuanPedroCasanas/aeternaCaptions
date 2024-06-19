using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using aeternaCaptions.src.model;

namespace aeternaCaptions.src.Utils
{

    public class Translator
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly IConfigurationSection TranslatorConfig;

        static Translator()
        {
            if (StaticConfiguration.config == null)
            {
                throw new InvalidOperationException("Configuration not initialized. Call StaticConfiguration.Initialize() before accessing configuration settings.");
            }

            TranslatorConfig = StaticConfiguration.config.GetSection("DeepL");

            // Set the base address for the translator
            httpClient.BaseAddress = new Uri(TranslatorConfig.GetValue<string>("BaseUri")!);

        }

        public static async Task<string?> TranslateSubtitles(string textContent, string sourceLanguage, string targetLanguage)
        {
            var subtitleEntries = new List<SubtitleEntry>();
            var translatedSubtitleEntries = new List<SubtitleEntry>();
            string pattern = @"(?<LineNumber>\d+)\s*(?<StartTime>\d{2}:\d{2}:\d{2},\d{3})\s*-->\s*(?<EndTime>\d{2}:\d{2}:\d{2},\d{3})\s*(?<Content>.*?)(?=\r?\n\d+\s*|\n)";

            MatchCollection matches = Regex.Matches(textContent, pattern, RegexOptions.Singleline);

            foreach (Match match in matches)
            {

                int lineNumber = int.Parse(match.Groups["LineNumber"].Value);
                TimeSpan startTime = TimeSpan.Parse(match.Groups["StartTime"].Value);
                TimeSpan endTime = TimeSpan.Parse(match.Groups["EndTime"].Value);
                string content = match.Groups["Content"].Value.Trim();
                
                SubtitleEntry entry = new SubtitleEntry
                {
                    LineNumber = lineNumber,
                    StartTime = startTime,
                    EndTime = endTime,
                    Content = content
                };
                subtitleEntries.Add(entry);

            }
            
            foreach (SubtitleEntry entry in subtitleEntries)
            {  
                SubtitleEntry? translatedEntry = await translateUsingDeepL(entry, sourceLanguage, targetLanguage);
                if(translatedEntry == null)
                {
                    return null;
                }
                translatedSubtitleEntries.Add(translatedEntry);
            }

            return parseSubtitleEntries(translatedSubtitleEntries);

        }


        private static async Task<SubtitleEntry?> translateUsingDeepL(SubtitleEntry entry, string sl, string tl)
        {
            
            var uriBuilder = new UriBuilder(httpClient.BaseAddress!);
            uriBuilder.Path += "INSERT URI PARAMETERS HERE";
            var finalUri = uriBuilder.Uri;
            using HttpResponseMessage response = await httpClient.GetAsync(finalUri);

            if(response.Content == null)
            {
                return null;
            }

            string translatedContent = await response.Content.ReadAsStringAsync();

            var translatedEntry = new SubtitleEntry {
                LineNumber = entry.LineNumber,
                StartTime = entry.StartTime,
                EndTime = entry.EndTime,
                Content = translatedContent
            };

            return translatedEntry;
        }

        private static string parseSubtitleEntries(List<SubtitleEntry> entries)
        {
            var translatedContent = new StringBuilder();

            foreach (SubtitleEntry entry in entries)
            {
                translatedContent.Append(entry.LineNumber);
                translatedContent.Append("\n");
                translatedContent.Append(entry.StartTime.ToString());
                translatedContent.Append(" --> ");
                translatedContent.Append(entry.EndTime.ToString());
                translatedContent.Append("\n");
                translatedContent.Append(entry.Content);
                translatedContent.Append("\n\n");
            }


            return translatedContent.ToString();
        }
    }
}