﻿using JazzApi.DTOs.Reto;
using JazzApi.Manager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JazzApi.Controllers.Reto
{
    [ApiController]
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TaskController : ControllerBase
    {
        private readonly TaskManager _manager;
        public TaskController(ApplicationDbContext Context, ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            string ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            string userName = Task.Run(async () => await userManager
            .FindByNameAsync(httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Username").Value)).Result.UserName; 
            string id = Task.Run(async () => await userManager
            .FindByNameAsync(httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Username").Value)).Result.Id;
            _manager = new TaskManager(Context, userName, ip,id);
        }
        [HttpGet("Task")]
        public async Task<ActionResult> IndexAsync() => Ok(await _manager.Get());
        [HttpPost("Task")]
        public async Task<ActionResult> SaveAsync([FromBody] TaskDTO data) => Ok(await _manager.Save(data));
        [HttpPut("Task")]
        public async Task<ActionResult> EditAsync(TaskDTO data) => Ok(await _manager.Edit(data));
        [HttpDelete("Task/{Id:long}")]
        public async Task<ActionResult> DeleteAsync(long Id) => Ok(await _manager.Delete(Id));

    }
}
