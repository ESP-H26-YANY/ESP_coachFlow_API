using CoachFlowApi.Application.DTOS;

namespace CoachFlowApi.Application.UseCases.User.Interfaces;

public interface ILoginUseCase
{
    Task<AuthDto> Execute(LoginDto dto);
}