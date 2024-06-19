using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aeternaCaptions.src.Dto.TranscriptDto;
using aeternaCaptions.src.model;

namespace aeternaCaptions.src.Interface
{
    public interface ITranscriptRepo
    {
        public Task<List<TranscriptCompactDto>> GetAllAsync();
        public Task<TranscriptDetailedDto?> GetByIdAsync(string transcriptId);
        public Task<Transcript> CreateAsync(Transcript transcript);
            public Task<bool> DeleteAsync(string transcriptId);
        public Task<TranscriptDetailedDto?> UpdateAsync(UpdateTranscriptDto transcript, string transcriptId);
        public Task<bool> TranscriptExistsAsync(string transcriptInternalId);
    }
}