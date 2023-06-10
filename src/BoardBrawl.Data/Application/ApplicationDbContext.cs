using BoardBrawl.Data.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardBrawl.Data.Application
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}