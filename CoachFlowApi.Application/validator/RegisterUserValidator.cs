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
            .MinimumLength(8)
            .MaximumLength(100)
            .Matches(@"[A-Z]").WithMessage("Au moins une majuscule requise")
            .Matches(@"[a-z]").WithMessage("Au moins une minuscule requise")
            .Matches(@"[0-9]").WithMessage("Au moins un chiffre requis")
            .Matches(@"[\W_]").WithMessage("Au moins un caractère spécial requis");

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(role => role == "user" || role == "coach")
            .WithMessage("Role must be either 'user' or 'coach'.");

    }
}