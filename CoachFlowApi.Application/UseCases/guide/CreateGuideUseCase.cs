using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.Guide.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.Results;
using GuideEntity = CoachFlowApi.Domain.Entities.Guide;

namespace CoachFlowApi.Application.UseCases.Guide;

public class CreateGuideUseCase : ICreateGuideUseCase
{
    private readonly IGuideRepository _guideRepository;
    private readonly ICoachRepository _coachRepository; 
    private readonly IValidator<CreateGuideDto> _validator;

    public CreateGuideUseCase(
        IGuideRepository guideRepository, 
        ICoachRepository coachRepository, 
        IValidator<CreateGuideDto> validator)
    {
        _guideRepository = guideRepository;
        _coachRepository = coachRepository;
        _validator = validator;
    }

    public async Task<GuideDto> Execute(CreateGuideDto dto)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var coach = await _coachRepository.FindByUserId(dto.UserId);
        if (coach == null)
            throw new Exception("Aucun profil de coach n'est associé à cet utilisateur.");

        var guide = new GuideEntity(
            coach.Id, 
            dto.Title,
            dto.Description,
            dto.Category,
            dto.LinkUrl,
            dto.CoverUrl, 
            dto.Price
        );

        var savedGuide = await _guideRepository.Add(guide);
        return new GuideDto(savedGuide);
    }
}