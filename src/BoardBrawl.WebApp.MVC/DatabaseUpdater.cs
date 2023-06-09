using BoardBrawl.Data.Application;
using BoardBrawl.Data.Identity;
using Microsoft.EntityFrameworkCore;

namespace BoardBrawl.WebApp.MVC
{
    public static class DatabaseUpdater
    {
        public static void Update(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var identityDbContext = services.GetRequiredService<IdentityDbContext>();
                    identityDbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    // Handle any errors that occurred during migration
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }

                try
                {
                    var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
                    applicationDbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    // Handle any errors that occurred during migration
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
        }
    }
}