using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aeternaCaptions.src.Dto.SubtitleFileDto;
using aeternaCaptions.src.model;

namespace aeternaCaptions.src.Mapper
{
    public static class SubtitleFileMapper
    {
        public static CreateSubtitleFileDto ToCreateDto(this SubtitleFile subtitleFile)
        {
            return new CreateSubtitleFileDto
            {
                SubtitleContent  = subtitleFile.SubtitleContent,
                SubtitleLanguage = subtitleFile.SubtitleLanguage,
                TranscriptId = subtitleFile.TranscriptId
            };
        }

        public static UpdateSubtitleFileDto ToUpdateDto(this SubtitleFile subtitleFile)
        {
            return new UpdateSubtitleFileDto
            {
                SubtitleContent  = subtitleFile.SubtitleContent,
                SubtitleLanguage = subtitleFile.SubtitleLanguage,
                TranscriptId = subtitleFile.TranscriptId
            };
        }

        public static SubtitleFile ToSubtitleFromCreateDto(this CreateSubtitleFileDto createSubtitleFileDto)
        {
            return new SubtitleFile
            {
                SubtitleContent  = createSubtitleFileDto.SubtitleContent,
                SubtitleLanguage = createSubtitleFileDto.SubtitleLanguage,
                TranscriptId = createSubtitleFileDto.TranscriptId
            };
        }
    }
}