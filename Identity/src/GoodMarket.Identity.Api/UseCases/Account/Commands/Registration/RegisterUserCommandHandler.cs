using GoodMarket.Identity.Api.UseCases.Account.Dtos;
using GoodMarket.Shared.Mediator;
using GoodMarket.Shared.Mediator.Command;
using GoodMarket.Shared.Result;

namespace GoodMarket.Identity.Api.UseCases.Account.Commands.Registration;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Result<RegistrationResultDto>>
{
    public Task<Result<RegistrationResultDto>> HandleAsync(RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            Result.SuccessResult()
                .WithData(new RegistrationResultDto(Guid.NewGuid().ToString()))
        );
    }
}