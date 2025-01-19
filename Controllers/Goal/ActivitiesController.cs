using JazzApi.DTOs.TRA;
using JazzApi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JazzApi.Controllers.Goal
{
    [Route("api/activities")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActivitiesController : ControllerBase
    {
        // GET: ActivitiesController
        private readonly IActivities _ActivityManager;
        public ActivitiesController(IActivities IActivities, IHttpContextAccessor httpContextAccessor) {
            _ActivityManager = IActivities;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll() => Ok(await _ActivityManager.GetAllActivities());

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id) => Ok(await _ActivityManager.GetActivityById(id));

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ActivityDTO activity) => Ok(await _ActivityManager.CreateActivity(activity));

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] ActivityDTO activity) => Ok(await _ActivityManager.UpdateActivity(activity));
        
        [HttpDelete]
        public async Task<ActionResult> Delete(long id) => Ok(await _ActivityManager.DeleteActivity(id));



    }
}
