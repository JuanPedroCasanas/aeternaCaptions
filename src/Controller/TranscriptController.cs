using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using aeternaCaptions.src.Interface;
using aeternaCaptions.src.model;
using aeternaCaptions.src.Utils;
using Microsoft.AspNetCore.Mvc;

namespace aeternaCaptions.src.Controller
{
    public class TranscriptController : ControllerBase
    {
        private readonly ISubtitleFileRepo _subtitleFileRepo;
        private readonly ITranscriptRepo _TranscriptRepo;

        public TranscriptController(ISubtitleFileRepo subtitleFileRepo, ITranscriptRepo TranscriptRepo)
        {
            _subtitleFileRepo = subtitleFileRepo;
            _TranscriptRepo = TranscriptRepo;    
        }
        

        
        [HttpGet]
        [Route("/transcripts/generate/{videoPath}/{videoTitle?}")]
        public async Task<IActionResult> GenerateCaptions([FromRoute] string videoPath, [FromRoute] string videoTitle)
        {
            if(videoTitle == null)
            {
                videoTitle = Path.GetFileName(videoPath);
            }
            Transcript? newTranscript = await MediaProcessor.GenerateCaptionsAsync(HttpUtility.UrlDecode(videoPath), videoTitle);
            if (newTranscript == null)
            {
                return StatusCode(500, "An error occurred while generating the captions for the video.");
            }
            if (!string.IsNullOrWhiteSpace(newTranscript.Error))
            {
                return StatusCode(500, $"An error occurred while generating the captions for the video: {newTranscript.Error}");
            }
            return Ok(await _TranscriptRepo.CreateAsync(newTranscript));
        }




        [HttpGet]
        [Route("/transcripts/get/{transcriptId}")]
        public async Task<IActionResult> GetTranscriptFromAssemblyAIServer([FromRoute] string transcriptId)
        {
            if(await _TranscriptRepo.TranscriptExistsAsync(transcriptId))
            {
                return Ok(await _TranscriptRepo.GetByIdAsync(transcriptId));
            }
            else
            {
                Transcript? transcript = await MediaProcessor.GetTranscriptFromAssemblyAIServerAsync(transcriptId);
                if (transcript == null)
                {
                    return NotFound("Transcript was not found: Please generate a valid Transcript before trying to fetch it.");
                }
                await _TranscriptRepo.CreateAsync(transcript);
                return Ok(transcript);
            }
        }
    }
}