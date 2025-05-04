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

        services.AddScoped<IHrAppSeeder, HrAppSeeder>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();

        services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();
        services.AddScoped<ITeamAuthorizationService, TeamAuthorizationService>();
    }
}
