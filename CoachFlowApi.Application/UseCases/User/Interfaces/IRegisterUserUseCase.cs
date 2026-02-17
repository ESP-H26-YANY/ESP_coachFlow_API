using CoachFlowApi.Application.DTOS;

namespace CoachFlowApi.Application.UseCases.User.Interfaces;

public interface IRegisterUserUseCase
{
    Task<UserDto> Execute(RegisterUserDto dto);
}