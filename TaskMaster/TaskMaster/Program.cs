using Shared.Models;
using Shared.MongoDb;
using Shared.MongoDb.DbService;
using TaskMaster.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddMongoDb(builder.Configuration)
    .AddMongoDbDataContext<Project>();

builder.Services
    .AddSingleton<IProjectsService,ProjectsService>();

var app = builder.Build();
app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/");
app.Run();