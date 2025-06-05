using Hangfire;
using Hangfire.Redis.StackExchange;
using HrApp.Application.Interfaces;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Authorizations;
using HrApp.Infrastructure.Presistance;
using HrApp.Infrastructure.Repositories;
using HrApp.Infrastructure.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HrApp.Infrastructure.Extentions;

public static class ServiceCollectionExtentions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HrApp");
        services.AddDbContext<HrAppContext>(options => options.UseSqlServer(connectionString));

        services.AddHangfire(config =>
            config.UseRedisStorage("localhost:6379"));

        services.AddHangfireServer();

        services.AddScoped<IHrAppSeeder, HrAppSeeder>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IWorkLogRepository, WorkLogRepository>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<IEmployeeRateRepository, EmployeeRateRepository>();
        services.AddScoped<ISalaryHistoryRepository, SalaryHistoryRepository>();
        services.AddScoped<IEmploymentHistoryRepository, EmploymentHistoryRepository>();
        services.AddScoped<IGoogleOAuthTokenRepository, GoogleOAuthTokenRepository>();
        services.AddScoped<ICalendarRepository, CalendarRepository>();
        services.AddScoped<IUserIpAddressRepository, UserIpAddressRepository>();
        services.AddScoped<IOverallRaportRepository, OverallRaportRepository>();
        services.AddScoped<IExellImportRepository, ExellImportRepository>();
        services.AddScoped<IApiLogRepository, ApiLogRepository>();

        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();
        services.AddScoped<ITeamAuthorizationService, TeamAuthorizationService>();
        services.AddScoped<IAssignmentAuthorizationService, AssignmentAuthorizationService>();
        services.AddScoped<ISalaryRepository, SalaryRepository>();
        services.AddScoped<IWorkLogExportHistoryRepository, WorkLogExportHistoryRepository>();
    }
}
