using CoachFlowApi.Application.DTOS;
using FluentValidation;

namespace CoachFlowApi.Application.Validators;

public class CreateGuideValidator : AbstractValidator<CreateGuideDto>
{
    public CreateGuideValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(20);
        RuleFor(x => x.LinkUrl).NotEmpty();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CoachId).NotEmpty();
    }
}