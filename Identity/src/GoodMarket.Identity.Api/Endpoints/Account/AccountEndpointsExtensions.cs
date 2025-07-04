namespace GoodMarket.Identity.Api.Endpoints.Account;

public static class AccountEndpointsExtensions
{
    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRegister();
        return app;
    }
}