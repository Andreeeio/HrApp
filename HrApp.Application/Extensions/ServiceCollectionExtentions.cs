using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HrApp.Application.Extensions;

public static class ServiceCollectionExtentions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(ServiceCollectionExtentions).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtentions).Assembly));
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtentions).Assembly).AddFluentValidationAutoValidation();
    }
}
