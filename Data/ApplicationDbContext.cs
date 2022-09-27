using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EDSU_JournalMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EDSU_JournalMS.Models.EDSUJournal> EDSUJournals { get; set; }
        public DbSet<EDSU_JournalMS.Models.JournalEditor> JournalEditors { get; set; }
        public DbSet<EDSU_JournalMS.Models.Comment> Comments { get; set; }


    }
}