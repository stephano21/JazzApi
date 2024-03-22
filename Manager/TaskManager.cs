using JazzApi.DTOs.Reto;
using JazzApi.Entities.Reto;

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
        
        public bool Save(TaskDTO data)
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
