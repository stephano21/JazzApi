using JazzApi.DTOs.Reto;
using JazzApi.Entities.Reto;
using Microsoft.EntityFrameworkCore;

namespace JazzApi.Manager
{
    public class TaskManager
    {
        private readonly ApplicationDbContext _context;
        public TaskManager(ApplicationDbContext Context)
        {
            _context = Context;
        }
        public List<TaskDTO> Get() => _context.TaskNotes.Select(x => new TaskDTO
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            UserId = x.UserId
        }).ToList();
        
        public async Task<bool> Save(TaskDTO data)
        {
            data.Title = data.Title.Trim();
            var ExisteTitle =await  _context.TaskNotes.Where(x => x.Title.ToLower() == data.Title.ToLower()).AnyAsync();
            if (ExisteTitle)
            {
                throw new Exception("El titulo ya existe");
            }
            var nuevo = new TaskNotes
            {
                Title = data.Title,
                Description = data.Description,
                UserId = "syuste"
            };
            await _context.AddAsync(nuevo);
            await _context.SaveChangesAsync();
            return true;
        }
        public bool Edit(TaskDTO data)
        {
            var nuevo = new TaskNotes
            {
                Title = data.Title,
                Description = data.Description,
                UserId = data.UserId
            };
            _context.AddAsync(nuevo);
            _context.SaveChangesAsync();
            return true;
        }
    }
}
