using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Models;
using Shared.MongoDb.DbService;
using Task = Shared.Models.Task;

namespace TaskMaster.Services
{
    public interface ISprintsService
    {
        Task<IEnumerable<Sprint>?> GetAllSprints();
        Task<IEnumerable<Sprint>?> GetManySprints(List<string> sprintIds);
        Task<IEnumerable<Sprint>?> GetSprintsByProjectId(string projectId);
        Task<IEnumerable<Task>?> GetTasksBySprintId(string sprintId);
        Task<Sprint?> GetSprintWithReferences(string id);
        Task<Sprint?> CreateSprint(string projectId ,Sprint sprint);
        Task<bool> UpdateSprint(string id, Sprint sprint);
        Task<bool> DeleteSprint(string id);
    }
    public class SprintsService: ISprintsService
    {
        private readonly IMongoDbDataContext<Sprint> _sprintDataContext;
        private readonly IMongoDbDataContext<Project> _projectDataContext;
        private readonly IMongoDbDataContext<Task> _taskDataContext;

        public SprintsService(IMongoDbDataContext<Sprint> sprintDataContext, IMongoDbDataContext<Project> projectDataContext, IMongoDbDataContext<Task> taskDataContext)
        {
            _sprintDataContext = sprintDataContext;
            _projectDataContext = projectDataContext;
            _taskDataContext = taskDataContext;
        }

        public async Task<IEnumerable<Sprint>?> GetAllSprints()
        {
            return await _sprintDataContext.GetAllAsync();
        }

        public async Task<IEnumerable<Sprint>?> GetManySprints(List<string> sprintIds)
        {
            var filter = Builders<Sprint>.Filter.In(x=>x.Id, sprintIds.Select(x=>new ObjectId(x)));
            return await _sprintDataContext.GetManyAsync(filter);
        }

        public async Task<IEnumerable<Sprint>?> GetSprintsByProjectId(string projectId)
        {
            var filter = Builders<Sprint>.Filter.Eq(x=>x.ProjectId.Id, new ObjectId(projectId));
            return await _sprintDataContext.GetManyAsync(filter);
        }
        
        public async Task<IEnumerable<Task>?> GetTasksBySprintId(string sprintId)
        {
            return (await GetSprintWithReferences(sprintId))?.Tasks;
        }

        public async Task<Sprint?> GetSprintWithReferences(string id)
        {
            var sprint = await _sprintDataContext.GetByIdAsync(new ObjectId(id));
            if (sprint != null)
            {
                var filterTasks = Builders<Task>.Filter.In(x=>x.Id, sprint.TaskIds.Select(x=>x.Id));
                sprint.Tasks = await _taskDataContext.GetManyAsync(filterTasks);
                sprint.Project = await _projectDataContext.GetByIdAsync(sprint.ProjectId.Id);
            }
        
            return sprint;
        }

        public async Task<Sprint?> CreateSprint(string projectId, Sprint sprint)
        {
            Sprint? result = null;
            var project = await _projectDataContext.GetByIdAsync(new ObjectId(projectId));
            if (project != null)
            {
                sprint.ProjectId = new (nameof(Project),project.Id);
                var insertedSprint = await _sprintDataContext.AddAsync(sprint);
                project.SprintIds.Add(new (nameof(Sprint),insertedSprint.Id));
                await _projectDataContext.UpdateAsync(project);
            
                result = sprint;
            }

            return result;
        }

        public async Task<bool> UpdateSprint(string id, Sprint sprint)
        {
            sprint.Id = new ObjectId(id);
            return await _sprintDataContext.UpdateAsync(sprint);
        }

        public async Task<bool> DeleteSprint(string id)
        {
            return await _sprintDataContext.DeleteAsync(new ObjectId(id));
        }
    }
}