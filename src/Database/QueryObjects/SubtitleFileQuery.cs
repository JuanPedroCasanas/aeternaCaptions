using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aeternaCaptions.src.model;

namespace aeternaCaptions.src.Database.QueryObjects
{
    public class SubtitleFileQuery
    {
        public required string SubtitleLanguage { get; set; } = string.Empty;
        public required string TranscriptId { get; set; } 
        
    }
}