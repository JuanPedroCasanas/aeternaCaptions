using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aeternaCaptions.src.Dto.TranscriptDto;
using aeternaCaptions.src.model;

namespace aeternaCaptions.src.Mapper
{
    public static class TranscriptMapper
    {
        public static TranscriptCompactDto ToCompactDto(this Transcript transcript)
        {
            return new TranscriptCompactDto 
            {
                Id = transcript.Id,
                Status = transcript.Status,
                Text = transcript.Text,
                NativeLanguageCode = transcript.NativeLanguageCode,
                Error = transcript.Error
            };
        }

        public static TranscriptDetailedDto ToDetailedDto(this Transcript transcript)
        {
            return new TranscriptDetailedDto
            {
                Id = transcript.Id,
                Status = transcript.Status,
                Text = transcript.Text,
                NativeLanguageCode = transcript.NativeLanguageCode,
                Error = transcript.Error,
                SubtitleFiles = transcript.SubtitleFiles,
                CreatedOn = transcript.CreatedOn,
            };
        }

        public static UpdateTranscriptDto ToUpdateDto(this Transcript transcript)
        {
            return new UpdateTranscriptDto
            {
                Title = transcript.Title
            };
        }

        public static Transcript ToTranscriptFromDetailedDto(this TranscriptDetailedDto transcriptDetailedDto)
        {
            return new Transcript
            {
                Id = transcriptDetailedDto.Id,
                Status = transcriptDetailedDto.Status,
                Text = transcriptDetailedDto.Text,
                NativeLanguageCode = transcriptDetailedDto.NativeLanguageCode,
                Error = transcriptDetailedDto.Error,
                SubtitleFiles = transcriptDetailedDto.SubtitleFiles,
                CreatedOn = transcriptDetailedDto.CreatedOn,
            };
        }
    }
}