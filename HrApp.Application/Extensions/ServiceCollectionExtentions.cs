﻿using FluentValidation;
using FluentValidation.AspNetCore;
using HrApp.Application.Assignment.Command.AddAssignment;
using HrApp.Application.Calendars.Command.CreateCalendarEvent;
using HrApp.Application.Interfaces;
using HrApp.Application.Services;
using HrApp.Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HrApp.Application.Extensions;

public static class ServiceCollectionExtentions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserContext, UserContext>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            var tokenKey = configuration["TokenKey"] ?? throw new Exception("Token key was not found");
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            opt.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies["jwt_token"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                }
            };

        }
        );

        services.AddFluentValidationAutoValidation(); 
        services.AddFluentValidationClientsideAdapters(); 
        services.AddValidatorsFromAssemblyContaining<AddAssignmentCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateCalendarEventCommandValidator>();


        services.AddHttpClient(); 

        services.AddHttpContextAccessor();
        services.AddAutoMapper(typeof(ServiceCollectionExtentions).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtentions).Assembly));
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtentions).Assembly).AddFluentValidationAutoValidation();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<IDeadlineChecker, DeadlineChecker>();
        services.AddTransient<ISalaryHistoryGenerator, SalaryHistoryGenerator>();
        services.AddTransient<IContractChecker, ContractChecker>();
        services.AddTransient<ICalendarService, CalendarService>();
        services.AddTransient<IGoogleAuthService, GoogleAuthService>();
        services.AddTransient<IIpAddressService, IpAddressService>();
        services.AddTransient<IRaportService, RaportService>();
    }
}