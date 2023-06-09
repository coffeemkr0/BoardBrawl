using BoardBrawl.WebApp.MVC.Areas.Identity.Data;
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
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.Migrate();
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