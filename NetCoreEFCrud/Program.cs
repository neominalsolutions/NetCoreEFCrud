using Microsoft.EntityFrameworkCore;
using NetCoreEFCrud.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// mvc controller servisi en �ste tan�ml� olmas� gerekir.

builder.Services.AddDbContext<NORTHWNDContext>(options =>
{
  // builder.Configuration ConfigurationManager s�n�f�n� return eder.
  options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindConn"));
});

// servis tan�mlar� app �zerinde konumland�r�lmal�d�r.

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
