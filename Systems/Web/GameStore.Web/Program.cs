using GameStore.Web.Services;
using GameStore.Web.Cloudinary;
using GameStore.Common.Helpers;
using GameStore.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
AutoMapperRegisterHelper.Register(services);
services.AddControllersWithViews();
services.AddScoped(sp => new HttpClient());

services.AddHttpContextAccessor();
services.AddCloudinarySettings(builder.Configuration);
services.AddScoped<IAccountService, AccountService>();
services.AddScoped<IGameService, GameService>();
services.AddScoped<ICommentService, CommentService>();
services.AddScoped<ICloudinaryService, CloudinaryService>();
services.AddScoped<ICookieService, CookieService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Main/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ExceptionsMiddleware>();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Main}/{action=Games}");

app.Run();
