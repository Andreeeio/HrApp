using DotNetEnv;
using Hangfire;
using HrApp.Application.Extensions;
using HrApp.Application.Interfaces;
using HrApp.Application.Mappings;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Extentions;
using HrApp.Infrastructure.Repositories;
using HrApp.Infrastructure.Seeder;
using HrApp.MVC.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();
var scope = app.Services.CreateScope();

var recurringJobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
var timezone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

recurringJobs.AddOrUpdate<IDeadlineChecker>(
    "check-deadlines",
    x => x.Check(),
    "0 7 * * *",
    new RecurringJobOptions
    {
        TimeZone = timezone
    });

recurringJobs.AddOrUpdate<ISalaryHistoryGenerator>(
    "generate-salary-history",
    x => x.GenerateSalaryHistoryAsync(),
    "0 1 24 * *", 
    new RecurringJobOptions
    {
        TimeZone = timezone
    });

recurringJobs.AddOrUpdate<IContractChecker>(
    "generate-salary-history",
    x => x.Check(),
    "0 0 1 * *",
    new RecurringJobOptions
    {
        TimeZone = timezone
    });

recurringJobs.AddOrUpdate<IRaportService>(
    "generate-salary-history",
    x => x.GenerateRaport(),
    "0 23 30 12 *",  
    new RecurringJobOptions
    {
        TimeZone = timezone
    });

Env.Load();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

    var seeder = scope.ServiceProvider.GetRequiredService<IHrAppSeeder>();
    await seeder.Seed();

    app.UseHsts();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
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

app.MapControllerRoute(
    name: "team",
    pattern: "{controller=Team}/{action=TeamInDept}/{id}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "team",
    pattern: "{controller=Team}/action={id}/employers")
    .WithStaticAssets();

app.Run();
