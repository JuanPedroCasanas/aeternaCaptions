using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aeternaCaptions.src.model
{
public class SubtitleEntry
{
    public int LineNumber { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Content { get; set; } = string.Empty;
}
}