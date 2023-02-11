using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using TaskMaster.Services;

namespace TaskMaster.Controllers;

public class ProjectsController : Controller
{
    private readonly IProjectsService _projectsService;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(IProjectsService projectsService, ILogger<ProjectsController> logger)
    {
        _projectsService = projectsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        IActionResult response;
        try
        {
            var projects = await _projectsService.GetAllProjects();
            response = Ok(projects);
            if (projects == null)
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
    public async Task<IActionResult> GetProject(string id)
    {
        IActionResult response;
        try
        {
            var project = await _projectsService.GetProjectWithReferences(id);
            response = Ok(project);
            if (project == null)
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
    public async Task<IActionResult> CreateProject([FromBody]Project project)
    {
        IActionResult response;
        try
        {
            response = Ok(await _projectsService.CreateProject(project));
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
            response = UnprocessableEntity(e);
        }

        return response;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProject(string id, [FromBody]Project project)
    {
        IActionResult response;
        try
        {
            var updatedProject = await _projectsService.UpdateProject(id, project);
            response = Ok(updatedProject);
            if (!updatedProject)
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
    public async Task<IActionResult> DeleteProject(string id)
    {
        IActionResult response;
        try
        {
            var deleted = await _projectsService.DeleteProject(id);
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