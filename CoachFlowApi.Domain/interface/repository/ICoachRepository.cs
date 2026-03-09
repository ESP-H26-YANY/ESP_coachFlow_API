using CoachFlowApi.Domain.Entities;

namespace CoachFlowApi.Domain.Interfaces.Repositories;

public interface ICoachRepository
{
    Task<Coach> Add(Coach coach);
}