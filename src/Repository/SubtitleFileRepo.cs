using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aeternaCaptions.src.Database;
using aeternaCaptions.src.Database.QueryObjects;
using aeternaCaptions.src.Dto.SubtitleFileDto;
using aeternaCaptions.src.Interface;
using aeternaCaptions.src.Mapper;
using aeternaCaptions.src.model;
using Microsoft.EntityFrameworkCore;

namespace aeternaCaptions.src.Repository
{
    public class SubtitleFileRepo : ISubtitleFileRepo
    {
        private readonly ApplicationDBContext _context;

        public SubtitleFileRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<SubtitleFile> CreateAsync(CreateSubtitleFileDto newSubtitleFile)
        {
            SubtitleFile newSub = newSubtitleFile.ToSubtitleFromCreateDto();
            await _context.SubtitleFiles.AddAsync(newSub);
            await _context.SaveChangesAsync();
            return newSub;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if(await SubtitleFileExistsAsync(id))
            {
                SubtitleFile subToRemove = await _context.SubtitleFiles.FirstAsync(s => s.Id == id);
                _context.SubtitleFiles.Remove(subToRemove);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<SubtitleFile>> GetAllAsync()
        {
            return await _context.SubtitleFiles.ToListAsync();
        }

        public async Task<SubtitleFile?> GetByIdAsync(int id)
        {
            return await _context.SubtitleFiles.Include(s => s.Transcript).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<SubtitleFile>> GetByQueryAsync(SubtitleFileQuery query)
        {
            IQueryable<SubtitleFile> subtitles = _context.SubtitleFiles.Include(c => c.Transcript).AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.TranscriptId))
            {
                subtitles = subtitles.Where(sub => sub.TranscriptId == query.TranscriptId);
            }
            if(!string.IsNullOrWhiteSpace(query.SubtitleLanguage))
            {
                subtitles = subtitles.Where(sub => sub.SubtitleLanguage.Equals(query.SubtitleLanguage));
            }
            return await subtitles.ToListAsync();
        }

        public async Task<bool> SubtitleFileExistsAsync(int id)
        {
            return  await _context.SubtitleFiles.FindAsync(id) != null;
        }

        public async Task<SubtitleFile?> UpdateAsync(UpdateSubtitleFileDto changedSubtitleFile, int subtitleFileId)
        {
            if(await SubtitleFileExistsAsync(subtitleFileId))
            {
                SubtitleFile subToUpdate = await _context.SubtitleFiles.FirstAsync(s => s.Id == subtitleFileId);
                subToUpdate.SubtitleContent = changedSubtitleFile.SubtitleContent;
                subToUpdate.SubtitleLanguage = changedSubtitleFile.SubtitleLanguage;
                subToUpdate.TranscriptId = changedSubtitleFile.TranscriptId;
                await _context.SaveChangesAsync();
                return subToUpdate;
            }
            return null;
        }
    }
}