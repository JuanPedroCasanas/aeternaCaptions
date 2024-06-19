using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace aeternaCaptions.src.model
{
    public class Transcript
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; } = String.Empty; // This id is the AssemblyAI server id
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = String.Empty;
        public string Text { get; set; } = String.Empty;

        [JsonPropertyName("language_code")]
        public string NativeLanguageCode { get; set; } = string.Empty;
        public string Error { get; set; } = String.Empty;
        public List<SubtitleFile> SubtitleFiles { get; set; } = new List<SubtitleFile>();
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}