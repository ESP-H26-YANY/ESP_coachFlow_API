using CoachFlowApi.Application.DTOS;

namespace CoachFlowApi.Application.UseCases.User.Interfaces;

public interface IGetUserProfileUseCase
{
    Task<UserDto> Execute(Guid userId);
}