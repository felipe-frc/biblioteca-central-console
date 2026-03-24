using Biblioteca.Services;
using Biblioteca.Web.Services;
using Biblioteca.Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BibliotecaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmprestimoAppService, EmprestimoAppService>();

builder.Services.AddScoped<IEmprestimoService, BibliotecaService>();

builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
    config.AddEventSourceLogger();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();