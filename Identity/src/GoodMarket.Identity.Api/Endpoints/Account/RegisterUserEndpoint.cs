using GoodMarket.Identity.Api.Shared.Extensions;
using GoodMarket.Shared.Result;
using GoodMarket.Shared.Mediator;
using GoodMarket.Identity.Api.UseCases.Account.Dtos;
using GoodMarket.Identity.Api.UseCases.Account.Commands.Registration;

namespace GoodMarket.Identity.Api.Endpoints.Account;

public static class RegisterUserEndpoint
{
    private const string Name = "Register";

    public static IEndpointRouteBuilder MapRegister(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Account.Register, async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new RegisterUserCommand("Nick", "nick@kvizhinadze.net");
            var result = await mediator.SendCommandAsync<RegisterUserCommand, Result<RegistrationResultDto>>(command, cancellationToken);

            return result.CustomResult();
        })
            .WithName(Name)
            .Produces<RegistrationResultDto>(StatusCodes.Status201Created)
            .Produces<IEnumerable<Error>>(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        return app;
    }
}
