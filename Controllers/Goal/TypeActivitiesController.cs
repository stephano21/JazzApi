using JazzApi.DTOs.TRA;
using JazzApi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JazzApi.Controllers.Goal
{
    [Route("api/activities/type")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TypeActivitiesController : ControllerBase
    {
        // GET: ActivitiesController
        private readonly ITypeActivities _TypeActivityManager;
        public TypeActivitiesController(ITypeActivities IActivities, IHttpContextAccessor httpContextAccessor) {
            _TypeActivityManager = IActivities;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll() => Ok(await _TypeActivityManager.GetAll());

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id) => Ok(await _TypeActivityManager.GetById(id));

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TypeActivityDTO activity) => Ok(await _TypeActivityManager.Create(activity));

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] TypeActivityDTO activity) => Ok(await _TypeActivityManager.Update(activity));
        
        [HttpDelete]
        public async Task<ActionResult> Delete(long id) => Ok(await _TypeActivityManager.Delete(id));



    }
}
