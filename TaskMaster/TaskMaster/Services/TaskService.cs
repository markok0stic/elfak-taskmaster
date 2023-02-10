using MongoDB.Bson;
using MongoDB.Driver;
using Shared.MongoDb.DbService;
using Task = Shared.Models.Task;

namespace TaskMaster.Services
{
    public interface ITasksService
    {
        Task<IEnumerable<Task>?> GetManyTasks(string sprintId);
        Task<Task?> GetTask(string id);
        Task<Task> CreateTask(Task task);
        Task<bool> UpdateTask(string id, Task task);
        Task<bool> DeleteTask(string id);
    }

    public class TaskService : ITasksService
    {
        private readonly IMongoDbDataContext<Task> _taskDataContext;

        public TaskService(IMongoDbDataContext<Task> taskDataContext)
        {
            _taskDataContext = taskDataContext;
        }

        public async Task<Task> CreateTask(Task task)
        {
            return await _taskDataContext.AddAsync(task);
        }

        public async Task<bool> DeleteTask(string id)
        {
            return await _taskDataContext.DeleteAsync(new ObjectId(id));
        }

        public async Task<IEnumerable<Task>?> GetManyTasks(string sprintId)
        {
            var filter = Builders<Task>.Filter.Eq("Task.Id", new ObjectId(sprintId));
            return await _taskDataContext.GetManyAsync(filter);
        }

        public Task<Task?> GetTask(string id)
        {
            return _taskDataContext.GetByIdAsync(new ObjectId(id));
        }

        public Task<bool> UpdateTask(string id, Task task)
        {
            task.Id = new ObjectId(id);
            return _taskDataContext.UpdateAsync(task);
        }
    }
}
