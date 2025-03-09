using JazzApi.DTOs.TRA;

namespace JazzApi.Interfaces
{
    public interface ITypeActivities
    {
        Task<List<TypeActivityDTO>> GetAll();
        Task<TypeActivityDTO> GetById(long id);
        Task<bool> Create(TypeActivityDTO goal);
        Task<bool> Update(TypeActivityDTO goal);
        Task<bool> Delete(long id);
    }
}
