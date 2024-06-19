using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aeternaCaptions.src.model
{
    public class SubtitleFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string SubtitleContent { get; set; }
        public required string SubtitleLanguage { get; set; }
        public required string TranscriptId { get; set; }
        [ForeignKey("TranscriptId")]
        public Transcript? Transcript { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}