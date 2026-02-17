using CoachFlowApi.Domain.Entities;

namespace CoachFlowApi.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAll();
    Task<User?> FindById(Guid id);
    Task<User?> FindByEmail(string email);
    Task<User> Add(User user);
    Task Delete(Guid id);
}