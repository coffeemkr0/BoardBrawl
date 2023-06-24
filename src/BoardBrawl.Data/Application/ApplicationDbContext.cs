using BoardBrawl.Data.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardBrawl.Data.Application
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Commander> Commanders { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}