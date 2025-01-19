using JazzApi.DTOs.TRA;

namespace JazzApi.Interfaces
{
    public interface IActivities
    {
        Task<List<ActivityDTO>> GetAllActivities();
        Task<ActivityDTO> GetActivityById(long id);
        Task<ActivityDTO> CreateActivity(ActivityDTO activity);
        Task<ActivityDTO> UpdateActivity(ActivityDTO activity);
        Task<bool> DeleteActivity(long id);

    }
}
