using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aeternaCaptions.src.Database;
using aeternaCaptions.src.Dto.TranscriptDto;
using aeternaCaptions.src.Interface;
using aeternaCaptions.src.Mapper;
using aeternaCaptions.src.model;
using Microsoft.EntityFrameworkCore;

namespace aeternaCaptions.src.Repository
{
    public class TranscriptRepo : ITranscriptRepo
    {
        private readonly ApplicationDBContext _context;

        public TranscriptRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Transcript> CreateAsync(Transcript transcript)
        {
            await _context.Transcripts.AddAsync(transcript);
            await _context.SaveChangesAsync();
            return transcript;
        }

        public async Task<bool> DeleteAsync(string TranscriptId)
        {
            if(await TranscriptExistsAsync(TranscriptId))
            {
                Transcript transcriptToRemove = await _context.Transcripts.FirstAsync(t => t.Id.Equals(TranscriptId));
                _context.Transcripts.Remove(transcriptToRemove);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<TranscriptCompactDto>> GetAllAsync()
        {
            return await _context.Transcripts.Select(transcript => transcript.ToCompactDto()).ToListAsync();
            
        }

        public async Task<TranscriptDetailedDto?> GetByIdAsync(string TranscriptId)
        {
            if(await TranscriptExistsAsync(TranscriptId))
            {
                Transcript transcript = await _context.Transcripts.FirstAsync(t => t.Id.Equals(TranscriptId));
                return transcript.ToDetailedDto();
            }
            return null;
        }

        public async Task<TranscriptDetailedDto?> UpdateAsync(UpdateTranscriptDto transcript, string TranscriptId)
        {
             if(await TranscriptExistsAsync(TranscriptId))
            {
                Transcript transcriptToUpdate = await _context.Transcripts.FirstAsync(t => t.Id.Equals(TranscriptId));
                transcriptToUpdate.Title = transcript.Title;
                await _context.SaveChangesAsync();
                return transcriptToUpdate.ToDetailedDto();
            }
            return null;
        }

        public async Task<bool> TranscriptExistsAsync(string TranscriptId)
        {
            Transcript? transcript = await _context.Transcripts.FirstOrDefaultAsync(t => t.Id.Equals(TranscriptId));
            return transcript != null;
        }
    }
}