using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using aeternaCaptions.src.model;
using Microsoft.EntityFrameworkCore;

namespace aeternaCaptions.src.Database
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<SubtitleFile> SubtitleFiles { get; set; }
        public DbSet<Transcript> Transcripts { get; set; }
    }
}