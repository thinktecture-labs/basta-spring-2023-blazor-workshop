using Microsoft.EntityFrameworkCore;

namespace WorkshopApi.Database
{
    public class ConferencesDbContext : DbContext
    {
        public DbSet<Conference> Conferences { get; set; }

        public ConferencesDbContext(DbContextOptions<ConferencesDbContext> options)
            : base(options)
        {
            
        }


    }
}
