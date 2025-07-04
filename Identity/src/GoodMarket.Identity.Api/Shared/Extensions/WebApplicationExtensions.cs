using GoodMarket.Identity.Api.Endpoints;
using Scalar.AspNetCore;

namespace GoodMarket.Identity.Api.Shared.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseScalar(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(endpointPrefix: "/docs", options =>
            {
                options.WithTitle("GoodMarket.Identity.Api")
                    .WithTheme(ScalarTheme.BluePlanet)
                    .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios);
            });
        }

        return app;
    }

    public static WebApplication CreateApiVersioning(this WebApplication app)
    {
        app.CreateApiVersionSet();
        return app;
    }
}