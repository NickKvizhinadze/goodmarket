using Asp.Versioning;
using Asp.Versioning.Builder;

namespace GoodMarket.Identity.Api.Endpoints;

public static class ApiVersioning
{
    public static ApiVersionSet VersionSet { get; private set; } = null!;

    public static IEndpointRouteBuilder CreateApiVersionSet(this IEndpointRouteBuilder app)
    {
        VersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();
    
        return app;
    }
}