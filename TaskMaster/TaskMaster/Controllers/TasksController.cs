using Microsoft.AspNetCore.Mvc;
using TaskMaster.Services;
using CTask = Shared.Models.Task;

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
        public async Task<IActionResult> GetSprintTasks(string sprintId)
        {
            IActionResult response;
            try
            {
                var tasks = await _taskService.GetManyTasks(sprintId);
                response = Ok(tasks);
                if (tasks == null)
                    response = NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError("", ex);
                response = UnprocessableEntity(ex);
            }
            return response;
        }

        [HttpGet]
        public async Task<IActionResult> GetSprint(string id)
        {
            IActionResult response;
            try
            {
                var task = await _taskService.GetTask(id);
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
        public async Task<IActionResult> CreateTask([FromBody] CTask task)
        {
            IActionResult response;
            try
            {
                response = Ok(await _taskService.CreateTask(task));
            }
            catch(Exception e)
            {
                _logger.LogError("", e);
                response = UnprocessableEntity(e);
            }
            return response;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(string id, [FromBody] CTask task)
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
