using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aeternaCaptions.src.model;

namespace aeternaCaptions.src.Dto.SubtitleFileDto
{
    public class CreateSubtitleFileDto
    {
        public required string SubtitleContent { get; set; }
        public required string SubtitleLanguage { get; set; }
        public required string TranscriptId { get; set; }
    }
}