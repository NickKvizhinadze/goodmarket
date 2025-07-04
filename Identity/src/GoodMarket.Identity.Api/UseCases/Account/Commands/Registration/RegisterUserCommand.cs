using FluentValidation;
using GoodMarket.Shared.Mediator.Command;
using GoodMarket.Identity.Api.UseCases.Account.Dtos;
using GoodMarket.Shared.Result;

namespace GoodMarket.Identity.Api.UseCases.Account.Commands.Registration;

public record RegisterUserCommand(string UserName, string Email) : ICommand<Result<RegistrationResultDto>>;


public class RegisterUserCommandCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotNull()
            .MinimumLength(10);
        //.WithMessage(string.Format(ErrorMessages.MinimumLength, 3));
    }
}