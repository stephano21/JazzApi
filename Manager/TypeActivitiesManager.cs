using JazzApi.DTOs.TRA;
using JazzApi.Entities.CAT;
using JazzApi.Entities.TRA;
using JazzApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JazzApi.Manager
{
    public class TypeActivitiesManager : ITypeActivities
    {
        private readonly ApplicationDbContext _context;
        private readonly string _user;
        private readonly string _ip;
        
        public TypeActivitiesManager(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = applicationDbContext;
            _user = httpContextAccessor.HttpContext.User.Identity.Name;
            _ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
        public async Task<bool> Create(TypeActivityDTO activity)
        {
            var newActivity = new TypeActivity
            {
                Type = activity.Type,
            };
            newActivity.Register(_user, _ip);
            _context.TypeActivity.Add(newActivity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            var activity = await _context.TypeActivity.FindAsync(id);
            if (activity == null)
            {
                return false;
            }
            activity.Inactive(_user,_ip);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TypeActivityDTO> GetById(long id) => await _context.TypeActivity
            .Where(x => x.Id == id && x.Active)
            .Select(x => new TypeActivityDTO
            {
                Id = x.Id,
                Type = x.Type,
            }).FirstOrDefaultAsync();
        
        public async Task<List<TypeActivityDTO>> GetAll()
         {
            return await _context.TypeActivity.Where(x=> x.Active).Select(x => new TypeActivityDTO
            {
                Id = x.Id,
                Type = x.Type,
            }).ToListAsync();
        }

        public async Task<bool> Update(TypeActivityDTO activity)
        {
            if (activity == null)
            {
                return false;
            }
           var currentActivity =await  _context.TypeActivity.Where(x => x.Id == activity.Id).FirstOrDefaultAsync();
            currentActivity.Type = activity.Type;
            currentActivity.Upgrade(_user, _ip);
            await _context.SaveChangesAsync();
            return true;


        }
    }
}
