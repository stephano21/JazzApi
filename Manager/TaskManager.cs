using JazzApi.DTOs.Reto;
using JazzApi.Entities.Reto;
using Microsoft.EntityFrameworkCore;

namespace JazzApi.Manager
{
    public class TaskManager
    {
        private readonly ApplicationDbContext _context;
        private readonly string _User;
        private readonly string _Ip;
        private readonly string _IdUser;
        public TaskManager(ApplicationDbContext Context, string Username, string Ip, string idUser)
        {
            _context = Context;
            _IdUser = idUser;
            _User = Username;
            _Ip = Ip;
            _IdUser = idUser;
        }
        public async Task<List<TaskDTO>> Get() =>await _context.TaskNotes.Select(x => new TaskDTO
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            UserId = x.UserId
        }).ToListAsync();
        
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
                UserId = _IdUser.ToString(),
            };
            await _context.TaskNotes.AddAsync(nuevo);
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
