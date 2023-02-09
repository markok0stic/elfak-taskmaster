using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Models;
using Shared.MongoDb.DbService;
using CTask = Shared.Models.Task;

namespace TaskMaster.Services
{
    public interface ITasksService
    {
        Task<IEnumerable<CTask>?> GetManyTasks(string sprintId);
        Task<CTask?> GetTask(string id);
        Task<CTask> CreateTask(CTask task);
        Task<bool> UpdateTask(string id, CTask task);
        Task<bool> DeleteTask(string id);
    }

    public class TasksServices : ITasksService
    {
        private readonly IMongoDbDataContext<CTask> _taskDataContext;

        public TasksServices(IMongoDbDataContext<CTask> taskDataContext)
        {
            _taskDataContext = taskDataContext;
        }

        public async Task<CTask> CreateTask(CTask task)
        {
            return await _taskDataContext.AddAsync(task);
        }

        public async Task<bool> DeleteTask(string id)
        {
            return await _taskDataContext.DeleteAsync(new ObjectId(id));
        }

        public async Task<IEnumerable<CTask>?> GetManyTasks(string sprintId)
        {
            var filter = Builders<CTask>.Filter.Eq("Task.Id", new ObjectId(sprintId));
            return await _taskDataContext.GetManyAsync(filter);
        }

        public Task<CTask?> GetTask(string id)
        {
            return _taskDataContext.GetByIdAsync(new ObjectId(id));
        }

        public Task<bool> UpdateTask(string id, CTask task)
        {
            return _taskDataContext.UpdateAsync(new ObjectId(id), task);
        }
    }
}
