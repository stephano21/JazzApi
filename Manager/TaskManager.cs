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
        public async Task<List<TaskDTO>> Get() =>await _context.TaskNotes.Where(x=> x.UserId.Equals(_IdUser)).Select(x => new TaskDTO
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
        public async Task<bool> Edit(TaskDTO data)
        {
           var duplicate =await _context.TaskNotes.Where(x => x.Title.ToLower() == data.Title.ToLower() && x.Id != data.Id).AnyAsync();
            if (duplicate)
            {
                throw new Exception("El titulo ya existe");
            }
           var Existe =await _context.TaskNotes.Where(x => x.Id == data.Id).FirstOrDefaultAsync();
            if (Existe == null)
            {
                throw new Exception("No existe el registro");
            }
            Existe.Title = data.Title;
            Existe.Description = data.Description;
            _context.TaskNotes.Update(Existe);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Delete(long Id)
        {
            var Existe =await _context.TaskNotes.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (Existe == null)
            {
                throw new Exception("No existe el registro");
            }
            _context.TaskNotes.Remove(Existe);
            await _context.SaveChangesAsync();
            return true;
        }   
    }
}
