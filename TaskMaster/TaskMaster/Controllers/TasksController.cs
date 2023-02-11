using Microsoft.AspNetCore.Mvc;
using TaskMaster.Services;
using Task = Shared.Models.Task;

namespace TaskMaster.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITasksService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITasksService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTask(string id)
        {
            IActionResult response;
            try
            {
                var task = await _taskService.GetTaskWithReferences(id);
                response = Ok(task);
                if (task == null)
                    response = NoContent();
            }
            catch(Exception e)
            {
                _logger.LogError("", e);
                response = UnprocessableEntity(e);
            }
            return response;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(string sprintId, [FromBody]Task task)
        {
            IActionResult response;
            try
            {
                var result = await _taskService.CreateTask(sprintId, task);
                response = Ok(result);
                if (result == null)
                {
                    response = BadRequest();
                }
            }
            catch(Exception e)
            {
                _logger.LogError("", e);
                response = UnprocessableEntity(e);
            }
            return response;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(string id, [FromBody]Task task)
        {
            IActionResult response;
            try
            {
                response = Ok(await _taskService.UpdateTask(id, task));
            }
            catch(Exception e) 
            {
                _logger.LogError("", e);
                response = UnprocessableEntity(e);
            }
            return response;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask(string id)
        {
            IActionResult response;
            try
            {
                response = Ok(await _taskService.DeleteTask(id));
            }
            catch (Exception e)
            {
                _logger.LogError("", e);
                response = UnprocessableEntity(e);
            }
            return response;
        }
    }
}
