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
    private readonly IValidator<CreateGuideDto> _validator;

    public CreateGuideUseCase(IGuideRepository guideRepository, IValidator<CreateGuideDto> validator)
    {
        _guideRepository = guideRepository;
        _validator = validator;
    }

    public async Task<GuideDto> Execute(CreateGuideDto dto)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var guide = new GuideEntity(
            dto.CoachId,
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