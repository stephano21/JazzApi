using JazzApi.DTOs.TRA;

namespace JazzApi.Interfaces
{
    public interface IGoal
    {
        Task<List<GoalDTO>> GetAllGoals();
        Task<GoalDTO> GetGoalById(long id);
        Task<GoalDTO> CreateGoal(GoalDTO goal);
        Task<GoalDTO> UpdateGoal(GoalDTO goal);
        Task<bool> DeleteGoal(long id);
    }
}
