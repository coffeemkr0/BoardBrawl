

using BoardBrawl.Core.AutoMapping;
using BoardBrawl.WebApp.MVC.Areas.Game.Hubs;
using BoardBrawl.WebApp.MVC.AutoMapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{

});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IMapper, AutoMapperMapper>();

builder.Services.AddScoped<BoardBrawl.Repositories.Lobby.IRepository, BoardBrawl.Repositories.Lobby.MemoryRepository>();
builder.Services.AddScoped<BoardBrawl.Services.Lobby.IService, BoardBrawl.Services.Lobby.Service>();

builder.Services.AddScoped<BoardBrawl.Repositories.Game.IRepository, BoardBrawl.Repositories.Game.MemoryRepository>();
builder.Services.AddScoped<BoardBrawl.Services.Game.IService, BoardBrawl.Services.Game.Service>();

var app = builder.Build();

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

app.MapHub<GameHub>("/GameHub");

app.Run();
