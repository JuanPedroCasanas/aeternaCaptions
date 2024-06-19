using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aeternaCaptions.src.Database.QueryObjects;
using aeternaCaptions.src.Dto.SubtitleFileDto;
using aeternaCaptions.src.model;

namespace aeternaCaptions.src.Interface
{
    public interface ISubtitleFileRepo
    {
        public Task<List<SubtitleFile>> GetAllAsync();
        public Task<SubtitleFile?> GetByIdAsync(int id);
        public Task<List<SubtitleFile>> GetByQueryAsync(SubtitleFileQuery query);
        public Task<SubtitleFile> CreateAsync(CreateSubtitleFileDto newSubtitleFile); 
        public Task<bool> DeleteAsync(int id);
        public Task<SubtitleFile?> UpdateAsync(UpdateSubtitleFileDto changedSubtitleFile, int subtitleFileId);
        public Task<bool> SubtitleFileExistsAsync(int id);
    }
}