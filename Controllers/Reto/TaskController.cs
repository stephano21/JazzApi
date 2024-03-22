using JazzApi.DTOs.Reto;
using JazzApi.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JazzApi.Controllers.Reto
{
    [Route("api")]
    public class TaskController : ControllerBase
    {
        private readonly TaskManager _manager;
        public TaskController(ApplicationDbContext Context)
        {
            _manager = new TaskManager(Context);
        }
        [HttpGet("Task")]
        public ActionResult Index()=> Ok(_manager.Get());
        [HttpPost("Task")]
        public ActionResult Save(TaskDTO data) => Ok( _manager.Save(data));
        [HttpPut("Task")]
        public ActionResult Edit(TaskDTO data) => Ok(_manager.Save(data));
        [HttpDelete("Task")]
        public ActionResult Delete(long Id) => Ok();
        
    }
}
