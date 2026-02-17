using CoachFlowApi.Application.DTOS;
using FluentValidation;

namespace CoachFlowApi.Application.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(40);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100);
        
        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(role => role == "user" || role == "coach")
            .WithMessage("Role must be either 'user' or 'coach'.");

    }
}