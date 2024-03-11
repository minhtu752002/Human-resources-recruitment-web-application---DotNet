using Microsoft.EntityFrameworkCore;
using ResourceEntity.Models;
using ResourceMNG;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(45);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRoleService, RoleService>();
var provider = builder?.Services?.BuildServiceProvider(); 
var config = provider?.GetService<IConfiguration>(); 
builder?.Services.AddDbContext<ResourceMngContext>(item => item.UseSqlServer(config?.GetConnectionString("ResourceMng")));
var app = builder.Build();

app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
