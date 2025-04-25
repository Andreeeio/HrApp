using HrApp.Application.Extensions;
using HrApp.Infrastructure.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.MapControllerRoute(
    name: "user",
    pattern: "{controller=User}/{action=Index}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "user",
    pattern: "{controller=User}/{action=CreateUser}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "user",
    pattern: "{controller=User}/{action=LoginUser}")
    .WithStaticAssets();


app.Run();
