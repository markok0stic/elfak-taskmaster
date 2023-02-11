using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Models;
using Shared.MongoDb.DbService;

namespace TaskMaster.Services;

public interface IProjectsService
{
    Task<IEnumerable<Project>?> GetAllProjects();
    Task<IEnumerable<Project>?> GetManyProjects(List<string> projectIds);
    Task<Project?> GetProjectWithReferences(string id);
    Task<Project> CreateProject(Project project);
    Task<bool> UpdateProject(string id, Project project);
    Task<bool> DeleteProject(string id);
}

public class ProjectsService: IProjectsService
{
    private readonly IMongoDbDataContext<Project> _projectDataContext;
    private readonly IMongoDbDataContext<Sprint> _sprintDataContext;
    
    public ProjectsService(IMongoDbDataContext<Project> projectDataContext, IMongoDbDataContext<Sprint> sprintDataContext)
    {
        _projectDataContext = projectDataContext;
        _sprintDataContext = sprintDataContext;
    }
    
    public async Task<IEnumerable<Project>?> GetAllProjects()
    {
        return await _projectDataContext.GetAllAsync();
    }

    public async Task<IEnumerable<Project>?> GetManyProjects(List<string> projectIds)
    {
        var filter = Builders<Project>.Filter.In(x => x.Id, projectIds.Select(x=>new ObjectId(x)));
        return await _projectDataContext.GetManyAsync(filter);
    }

    public async Task<Project?> GetProjectWithReferences(string id)
    {
        var project = await _projectDataContext.GetByIdAsync(new ObjectId(id));
        if (project != null)
        {
            var filter = Builders<Sprint>.Filter.In(x=>x.Id, project.SprintIds.Select(x=>x.Id));
            project.Sprints = await _sprintDataContext.GetManyAsync(filter);
        }
        
        return project;
    }

    public async Task<Project> CreateProject(Project project)
    {
        return await _projectDataContext.AddAsync(project);
    }

    public async Task<bool> UpdateProject(string id, Project project)
    {
        bool result = false;
        var proj = await _projectDataContext.GetByIdAsync(new ObjectId(id));
        if (proj != null)
        {
            proj.Name = project.Name;
            result = await _projectDataContext.UpdateAsync(proj);
        }
        return result;
    }

    public async Task<bool> DeleteProject(string id)
    {
        return await _projectDataContext.DeleteAsync(new ObjectId(id));
    }
}