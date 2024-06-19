using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using aeternaCaptions.src.Database.QueryObjects;
using aeternaCaptions.src.Dto.SubtitleFileDto;
using aeternaCaptions.src.Dto.TranscriptDto;
using aeternaCaptions.src.Interface;
using aeternaCaptions.src.model;
using aeternaCaptions.src.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using YoutubeExplode;

namespace aeternaCaptions.src.Controller
{
    [Route("api/subtitles")]
    public class SubtitleFileController : ControllerBase
    {
        private readonly ISubtitleFileRepo _subtitleFileRepo;
        private readonly ITranscriptRepo _TranscriptRepo;

        public SubtitleFileController(ISubtitleFileRepo subtitleFileRepo, ITranscriptRepo TranscriptRepo)
        {
            _subtitleFileRepo = subtitleFileRepo;
            _TranscriptRepo = TranscriptRepo;    
        }


        [HttpGet]
        [Route("/subtitles/{transcriptId}/{languageStringOrdinal}")] //Corregir esta logica a la hora de generar traducciones
        public async Task<IActionResult> GetSubtitle([FromRoute] string transcriptId, [FromRoute] string languageStringOrdinal)
        {
            TranscriptDetailedDto? transcript = await _TranscriptRepo.GetByIdAsync(transcriptId);

            if(transcript == null)
            {
                return NotFound("Transcript not found: To get a subtitle file first generate a valid transcript from a video");
            }

            SubtitleFileQuery query = new SubtitleFileQuery
            {
                TranscriptId = transcriptId,
                SubtitleLanguage = languageStringOrdinal
            };

            List<SubtitleFile> availableSubtitles = await _subtitleFileRepo.GetByQueryAsync(query);

            if (availableSubtitles.Any())
            {
                return Ok(availableSubtitles);
            }
            
            else
            {
                return await GenerateSubtitleFile(transcript, languageStringOrdinal);
            }
        }

        [HttpGet]
        [Route("/test")]
        public async Task<IActionResult> Test()
        {
            var subs = @"
                1
                00:00:00,240 --> 00:00:03,542
                So these egg loop knots are perfect for soft baits. And I'm

                2
                00:00:03,558 --> 00:00:05,718
                going to show you guys how to tie this knot real quick. So we're going
                ";

                string? translatedSubtitles = await Translator.TranslateSubtitles(subs, "en", "en");
                return Ok(translatedSubtitles);

            
        }

        [HttpGet]
        [Route("/BurnSubsTest/{videoPath}/{subtitleFileId}")]
        public async Task<IActionResult> BurnSubsTest([FromRoute] string videoPath, [FromRoute] int subtitleFileId)
        {
            SubtitleFile? sub = await _subtitleFileRepo.GetByIdAsync(subtitleFileId);
            if(sub == null)
            {
                return NotFound("Subtitle file was not found.");
            }
            Byte[] finalVideo = await MediaProcessor.BurnSubtitlesToVideo(HttpUtility.UrlDecode(videoPath), sub);
            return new FileContentResult(finalVideo, "application/octet-stream");
        }

        [HttpDelete]
        [Route("/subtitles/{subtitleId}")]
        public async Task<IActionResult> Delete(int subtitleId)
        {
            if(await _subtitleFileRepo.DeleteAsync(subtitleId))
            {
                return Ok();
            }
            return NotFound();
        }



        private async Task<IActionResult> GenerateSubtitleFile(TranscriptDetailedDto transcript, string translationLanguage)
        {
                SubtitleFile? finalSubFile = null;

                var query = new SubtitleFileQuery
                    {
                        TranscriptId = transcript.Id,
                        SubtitleLanguage = transcript.NativeLanguageCode
                    };

                List<SubtitleFile> subs = await _subtitleFileRepo.GetByQueryAsync(query);


                if(!subs.Any()) //If the language i want subtitles for is the same than the video one or there isn't any base subs generated 
                {
                    string subContent = await MediaProcessor.ExportSubtitlesAsync(transcript.Id, "srt"); // pass language as well here
                    CreateSubtitleFileDto newSub = new CreateSubtitleFileDto
                    {
                        SubtitleContent = subContent,
                        SubtitleLanguage = translationLanguage,
                        TranscriptId = transcript.Id
                    };
                    finalSubFile = await _subtitleFileRepo.CreateAsync(newSub);
                }
                

                if(translationLanguage == transcript.NativeLanguageCode)
                {
                    if(finalSubFile != null)
                    {
                        return Ok(finalSubFile);
                    }
                    else //If the sub file was not generated and the lang is the same as the transcript one then i already have it in the db
                    {
                        finalSubFile = subs.FirstOrDefault();
                        if(finalSubFile != null)
                        {
                            return Ok(finalSubFile);
                        }
                        else
                        {
                            return NotFound("Subtitle was not generated and not found, please try generating a new transcript and subtitle");
                        }
                    }
                }
                
                else
                {
                    if(finalSubFile == null)
                    {
                        finalSubFile = subs.FirstOrDefault();
                    }
                    if(finalSubFile == null)
                    {
                        return NotFound("Subtitle was not generated and not found, please try generating a new transcript and subtitle");
                    }

                    string? translatedSubtitlesStr = await Translator.TranslateSubtitles(finalSubFile.SubtitleContent, transcript.NativeLanguageCode, translationLanguage);
                    if(translatedSubtitlesStr == null)
                    {
                        return StatusCode(500, "An error has ocurred during the translation process, please try again");
                    }
                    
                    var translatedSubFile = new CreateSubtitleFileDto 
                    {
                        SubtitleContent = translatedSubtitlesStr,
                        SubtitleLanguage = translationLanguage,
                        TranscriptId = transcript.Id
                    };

                    await _subtitleFileRepo.CreateAsync(translatedSubFile);

                    return Ok(translatedSubtitlesStr);
                }
        }

    }
}