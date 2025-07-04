using Asp.Versioning;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GoodMarket.Shared.Mediator;
using GoodMarket.Shared.Mediator.Pipeline;
using GoodMarket.Identity.Api.Data;
using GoodMarket.Identity.Api.Pipelines;
using GoodMarket.Identity.Api.Domain.Users;
using GoodMarket.Identity.Api.Shared.OpenApi;
using GoodMarket.Identity.Api.UseCases.Account.Commands.Registration;

namespace GoodMarket.Identity.Api.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(b =>
            b.UseNpgsql(configuration.GetConnectionString(nameof(ApplicationDbContext)))
        );

        return services;
    }
    
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddIdentityServer()
            .AddAspNetIdentity<ApplicationUser>()
            .AddInMemoryClients(IdentityConfiguration.Clients)
            .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
            .AddInMemoryApiResources(IdentityConfiguration.ApiResources)
            .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
            .AddTestUsers(IdentityConfiguration.TestUsers)
            .AddDeveloperSigningCredential();

        return services;
    }

    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediator(typeof(IApiMarker).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(RegisterUserCommand), ServiceLifetime.Transient);
        return services;
    }
    
    public static IServiceCollection AddOpenApiCustom(this IServiceCollection services)
    {
        services.AddOpenApi(options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
        services.AddEndpointsApiExplorer();
        
        return services;
    }

    public static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(2);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"));
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}