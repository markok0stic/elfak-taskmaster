using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using TaskMaster.Services;

namespace TaskMaster.Controllers;

public class SprintsController: Controller
{
    private readonly ISprintsService _sprintsService;
    private readonly ILogger<SprintsController> _logger;

    public SprintsController(ISprintsService sprintsService, ILogger<SprintsController> logger)
    {
        _logger = logger;
        _sprintsService = sprintsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSprints()
    {
        IActionResult response;
        try
        {
            var sprints = await _sprintsService.GetAllSprints();
            response = Ok(sprints);
            if (sprints == null)
            {
                response = NoContent();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
            response = UnprocessableEntity(e);
        }
        
        return response;
    }

    [HttpGet]
    public async Task<IActionResult> GetSprint(string id)
    {
        IActionResult response;
        try
        {
            var sprint = await _sprintsService.FetchSprintWithReferences(id);
            response = Ok(sprint);
            if (sprint == null)
            {
                response = NoContent();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
            response = UnprocessableEntity(e);
        }

        return response;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSprint(string projectId, [FromBody]Sprint sprint)
    {
        IActionResult response;
        try
        {
            var result = await _sprintsService.CreateSprint(projectId, sprint);
            response = Ok(result);
            if (result == null)
            {
                response = BadRequest();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
            response = UnprocessableEntity(e);
        }

        return response;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSprint(string id, [FromBody]Sprint sprint)
    {
        IActionResult response;
        try
        {
            var updatedSprint = await _sprintsService.UpdateSprint(id, sprint);
            response = Ok(updatedSprint);
            if (!updatedSprint)
            {
                response = NoContent();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
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
            var deleted = await _sprintsService.DeleteSprint(id);
            response = Ok(deleted);
            if (!deleted)
            {
                response = NoContent();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
            response = UnprocessableEntity(e);
        }
        
        return response;
    }
}