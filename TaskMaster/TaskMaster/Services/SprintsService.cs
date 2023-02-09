using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Models;
using Shared.MongoDb.DbService;

namespace TaskMaster.Services
{
    public interface ISprintsService
    {
        Task<IEnumerable<Sprint>?> GetManySprints(string projectId);
        Task<Sprint?> GetSprint(string id);
        Task<Sprint> CreateSprint(Sprint sprint);
        Task<bool> UpdateSprint(string id, Sprint sprint);
        Task<bool> DeleteSprint(string id);
    }

    public class SprintsService : ISprintsService
    {
        private readonly IMongoDbDataContext<Sprint> _sprintDataContext;

        public SprintsService(IMongoDbDataContext<Sprint> sprintDataContext)
        {
            _sprintDataContext = sprintDataContext;
        }

        public async Task<Sprint> CreateSprint(Sprint sprint)
        {
           return await _sprintDataContext.AddAsync(sprint);
        }

        public async Task<bool> DeleteSprint(string id)
        {
            return await _sprintDataContext.DeleteAsync(new ObjectId(id));
        }

        public async Task<IEnumerable<Sprint>?> GetManySprints(string projectId)
        {
            var filter = Builders<Sprint>.Filter.Eq("Sprint.Id", new ObjectId(projectId));
            return await _sprintDataContext.GetManyAsync(filter);
        }

        public async Task<Sprint?> GetSprint(string id)
        {
            return await _sprintDataContext.GetByIdAsync(new ObjectId(id));
        }

        public async Task<bool> UpdateSprint(string id, Sprint sprint)
        {
            return await _sprintDataContext.UpdateAsync(new ObjectId(id), sprint);
        }
    }
}
