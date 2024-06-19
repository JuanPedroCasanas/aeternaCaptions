using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aeternaCaptions.src.model;

namespace aeternaCaptions.src.Dto.TranscriptDto
{
    public class TranscriptCompactDto
    {
        public string Id { get; set; } = String.Empty; // This id is the AssemblyAI server id
        public string Status { get; set; } = String.Empty;
        public string Text { get; set; } = String.Empty;
        public required string NativeLanguageCode { get; set; }
        public string Error { get; set; } = String.Empty;
    }
}