using MongoDB.Bson;
using Shared.Models;
using Shared.MongoDb.DbService;
using Task = System.Threading.Tasks.Task;

namespace TaskMaster.Services;

public interface IProjectsService
{
    Task<IEnumerable<Project>?> GetAllProjects();
    Task<Project?> GetProject(string id);
    Task<Project?> FetchProjectWithReferences(string id);
    Task<Project> CreateProject(Project project);
    Task<bool> UpdateProject(string id, Project project);
    Task<bool> DeleteProject(string id);
}

public class ProjectsService: IProjectsService
{
    private readonly IMongoDbDataContext<Project> _projectDataContext;
    
    public ProjectsService(IMongoDbDataContext<Project> projectDataContext)
    {
        _projectDataContext = projectDataContext;
    }
    
    public async Task<IEnumerable<Project>?> GetAllProjects()
    {
        return await _projectDataContext.GetAllAsync();
    }

    public async Task<Project?> GetProject(string id)
    {
        return await _projectDataContext.GetByIdAsync(new ObjectId(id));
    }

    public async Task<Project?> FetchProjectWithReferences(string id)
    {
        return await _projectDataContext.FetchWithReferences(new ObjectId(id));
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