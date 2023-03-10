using Shared.Models;
using Shared.MongoDb;
using Shared.MongoDb.DbService;
using TaskMaster.Services;
using Task = Shared.Models.Task;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddMongoDb(builder.Configuration)
    .AddMongoDbDataContext<Project>()
    .AddMongoDbDataContext<Sprint>()
    .AddMongoDbDataContext<Task>();

builder.Services
    .AddSingleton<IProjectsService, ProjectsService>()
    .AddSingleton<ISprintsService, SprintsService>()
    .AddSingleton<ITasksService, TaskService>();

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