using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using TaskMaster.Services;

namespace TaskMaster.Controllers
{
    public class SprintsController : Controller
    {
        private readonly ISprintsService _sprintsService;
        private readonly ILogger<SprintsController> _logger;

        public SprintsController(ISprintsService sprintsService, ILogger<SprintsController> logger)
        {
            _sprintsService = sprintsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectSprints(string projectId) 
        {
            IActionResult response;
            try
            {
                var sprints = await _sprintsService.GetManySprints(projectId);
                response = Ok(sprints);
                if (sprints == null)
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
                var sprint = await _sprintsService.GetSprint(id);
                response = Ok(sprint);
                if (sprint == null)
                    response = NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex);
                response = UnprocessableEntity(ex);
            }
            return response;
        }

        [HttpPost]
        public async Task<IActionResult> CreteSprint([FromBody] Sprint sprint)
        {
            IActionResult response;
            try
            {
                response = Ok(await _sprintsService.CreateSprint(sprint));
            }
            catch(Exception e)
            {
                _logger.LogError("", e);
                response = UnprocessableEntity(e);
            }
            return response;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSprint(string id, [FromBody] Sprint sprint)
        {
            IActionResult response;
            try
            {
                response = Ok(await _sprintsService.UpdateSprint(id, sprint));
            }
            catch(Exception e)
            {
                _logger.LogError("", e);
                response = UnprocessableEntity(e);
            }
            return response;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSprint(string id)
        {
            IActionResult response;
            try
            {
                response = Ok(await _sprintsService.DeleteSprint(id));
            }
            catch(Exception e)
            {
                _logger.LogError("", e);
                response = UnprocessableEntity(e);
            }
            return response;
        }
    }
}
