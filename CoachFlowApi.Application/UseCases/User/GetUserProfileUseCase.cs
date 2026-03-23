using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.User.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.User;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserProfileUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Execute(Guid userId)
    {
        var user = await _userRepository.FindById(userId);
        if (user == null) throw new Exception("Utilisateur introuvable.");

        return new UserDto(user);
    }
}