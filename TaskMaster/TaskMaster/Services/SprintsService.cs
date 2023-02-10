using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Models;
using Shared.MongoDb.DbService;

namespace TaskMaster.Services
{
    public interface ISprintsService
    {
        Task<IEnumerable<Sprint>?> GetAllSprints();
        Task<Sprint?> GetSprint(string id);
        Task<IEnumerable<Sprint>?> GetManySprints(string projectId);
        Task<Sprint?> FetchSprintWithReferences(string id);
        Task<Sprint?> CreateSprint(string projectId ,Sprint sprint);
        Task<bool> UpdateSprint(string id, Sprint sprint);
        Task<bool> DeleteSprint(string id);
    }
    public class SprintsService: ISprintsService
    {
        private readonly IMongoDbDataContext<Sprint> _sprintDataContext;
        private readonly IMongoDbDataContext<Project> _projectDataContext;

        public SprintsService(IMongoDbDataContext<Sprint> sprintDataContext, IMongoDbDataContext<Project> projectDataContext)
        {
            _sprintDataContext = sprintDataContext;
            _projectDataContext = projectDataContext;
        }

        public async Task<IEnumerable<Sprint>?> GetAllSprints()
        {
            return await _sprintDataContext.GetAllAsync();
        }

        public async Task<Sprint?> GetSprint(string id)
        {
            return await _sprintDataContext.GetByIdAsync(new ObjectId(id));
        }

        public async Task<IEnumerable<Sprint>?> GetManySprints(string projectId)
        {
            var filter = Builders<Sprint>.Filter.Eq("Sprint.Id", new ObjectId(projectId));
            return await _sprintDataContext.GetManyAsync(filter);
        }
        public async Task<Sprint?> FetchSprintWithReferences(string id)
        {
            return await _sprintDataContext.FetchWithReferences(new ObjectId(id));
        }

        public async Task<Sprint?> CreateSprint(string projectId, Sprint sprint)
        {
            Sprint? result = null;
            var project = await _projectDataContext.GetByIdAsync(new ObjectId(projectId));
            if (project != null)
            {
                sprint.ProjectId = new MongoDBRef(nameof(Project), projectId);
                var insertedSprint = await _sprintDataContext.AddAsync(sprint);
                project.SprintIds.Add(new MongoDBRef(nameof(Sprint),insertedSprint.Id));
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