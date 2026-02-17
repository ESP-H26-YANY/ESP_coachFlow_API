using CoachFlowApi.Domain.Entities;

namespace CoachFlowApi.Application.DTOS;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }

    public UserDto() { }

    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        Role = user.Role;
    }
}