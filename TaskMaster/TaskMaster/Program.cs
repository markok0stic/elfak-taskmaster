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
    .AddMongoDbDataContext<Class>();
builder.Services
    .AddSingleton<Service>();
/*builder.Services
    .AddTransient<>();*/

var app = builder.Build();
app.UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/");
app.Run();