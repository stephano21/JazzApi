using JazzApi.DTOs.TRA;
using JazzApi.Entities.TRA;
using JazzApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JazzApi.Manager
{
    public class ActivitiesManager: IActivities
    {
        private readonly ApplicationDbContext _context;
        private readonly string _user;
        private readonly string _ip;
        
        public ActivitiesManager(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = applicationDbContext;
            _user = httpContextAccessor.HttpContext.User.Identity.Name;
            _ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
        public async Task<ActivityDTO> CreateActivity(ActivityDTO activity)
        {
            var newActivity = new Activity
            {
                Name = activity.Name,
                Description = activity.Description,
                Expiration = activity.Expiration,
                Frecuency = activity.Frecuency,
                TypeId = activity.TypeId
            };
            newActivity.Register(_user, _ip);
            _context.Activity.Add(newActivity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<bool> DeleteActivity(long id)
        {
            var activity = await _context.Activity.FindAsync(id);
            if (activity == null)
            {
                return false;
            }
            activity.Inactive(_user,_ip);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ActivityDTO> GetActivityById(long id) => await _context.Activity
            .Where(x => x.Id == id && x.Active)
            .Select(x => new ActivityDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Expiration = x.Expiration,
                Frecuency = x.Frecuency,
                TypeId = x.TypeId
            }).FirstOrDefaultAsync();
        
        public async Task<List<ActivityDTO>> GetAllActivities()
        {
            return await _context.Activity.Where(x=> x.Active).Select(x => new ActivityDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Expiration = x.Expiration,
                Frecuency = x.Frecuency,
                TypeId = x.TypeId
            }).ToListAsync();
        }

        public async Task<ActivityDTO> UpdateActivity(ActivityDTO activity)
        {
            if (activity == null)
            {
                return null;
            }
           var currentActivity =await  _context.Activity.Where(x => x.Id == activity.Id).FirstOrDefaultAsync();
            currentActivity.Name = activity.Name;
            currentActivity.Description = activity.Description;
            currentActivity.Expiration = activity.Expiration;
            currentActivity.Frecuency = activity.Frecuency;
            currentActivity.TypeId = activity.TypeId;
            currentActivity.Upgrade(_user, _ip);
            await _context.SaveChangesAsync();
            return activity;


        }
    }
}
