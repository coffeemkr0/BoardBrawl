

using BoardBrawl.Core.AutoMapping;
using BoardBrawl.WebApp.MVC.Areas.Game.Hubs;
using BoardBrawl.WebApp.MVC.AutoMapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using BoardBrawl.Data.Identity;
using BoardBrawl.Data.Application;
using Microsoft.AspNetCore.Identity.UI.Services;
using BoardBrawl.WebApp.MVC.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

var identityConnetionString = builder.Configuration.GetConnectionString("IdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityDbContextConnection' not found.");
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseMySql(identityConnetionString, ServerVersion.Parse("10.11.3")));

var applicationConnectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.EnableSensitiveDataLogging(true);
        options.UseMySql(applicationConnectionString, ServerVersion.Parse("10.11.3"));
    });

builder.Services.AddDefaultIdentity<IdentityUser>(
    options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 0;
    })
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = Environment.GetEnvironmentVariable("BoardBrawl_Google_ClientId");
    googleOptions.ClientSecret = Environment.GetEnvironmentVariable("BoardBrawl_Google_ClientSecret");
});

#if DEBUG
builder.Services.AddAuthentication(DebugAuthHandler.AuthenticationScheme)
                .AddScheme<DebugAuthHandlerOptions, DebugAuthHandler>(DebugAuthHandler.AuthenticationScheme, options => { });
#endif

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{

});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IMapper, AutoMapperMapper>();

builder.Services.AddScoped<BoardBrawl.Repositories.Lobby.IRepository, BoardBrawl.Repositories.Lobby.EntityFrameworkRepository>();
builder.Services.AddScoped<BoardBrawl.Services.Lobby.IService, BoardBrawl.Services.Lobby.Service>();

builder.Services.AddScoped<BoardBrawl.Repositories.Game.IRepository, BoardBrawl.Repositories.Game.EntityFrameworkRepository>();
builder.Services.AddScoped<BoardBrawl.Services.Game.IService, BoardBrawl.Services.Game.Service>();

builder.Services.AddScoped<BoardBrawl.Services.Test.TestService, BoardBrawl.Services.Test.TestService>();

builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "Main",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Lobby",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Game",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapHub<GameHub>("/GameHub");

DatabaseUpdater.Update(app);

app.Run();
