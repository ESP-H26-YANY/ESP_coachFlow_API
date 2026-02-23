using CoachFlowApi.Application.DTOS;
using FluentValidation;

namespace CoachFlowApi.Application.Validators;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
            
        RuleFor(x => x.Password)
            .NotEmpty();
    }
}