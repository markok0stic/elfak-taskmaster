using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Models;
using Shared.Models.Bson;
using Shared.MongoDb.DbService;
using Task = Shared.Models.Task;

namespace TaskMaster.Services
{
    public interface ITasksService
    {
        Task<IEnumerable<Task>?> GetManyTasks(List<string> taskIds);
        Task<Task?> GetTaskWithReferences(string id);
        Task<Task?> CreateTask(string sprintId, Task task);
        Task<bool> UpdateTask(string id, Task task);
        Task<bool> DeleteTask(string id);
    }

    public class TaskService: ITasksService
    {
        private readonly IMongoDbDataContext<Task> _taskDataContext;
        private readonly IMongoDbDataContext<Sprint> _sprintDataContext;
        private readonly IMongoDbDataContext<Project> _projectDataContext;

        public TaskService(IMongoDbDataContext<Task> taskDataContext, IMongoDbDataContext<Sprint> sprintDataContext, IMongoDbDataContext<Project> projectDataContext)
        {
            _taskDataContext = taskDataContext;
            _sprintDataContext = sprintDataContext;
            _projectDataContext = projectDataContext;
        }

        public async Task<IEnumerable<Task>?> GetManyTasks(List<string> taskIds)
        {
            var filter = Builders<Task>.Filter.In(x=>x.Id, taskIds.Select(x=>new ObjectId(x)));
            return await _taskDataContext.GetManyAsync(filter);
        }
        
        public async Task<Task?> GetTaskWithReferences(string id)
        {
            var task = await _taskDataContext.GetByIdAsync(new ObjectId(id));
            if (task != null)
            {
                var filterSprints = Builders<Sprint>.Filter.In(x=>x.Id, task.SprintIds.Select(x=>x.Id));
                task.Sprints = await _sprintDataContext.GetManyAsync(filterSprints);
                task.Project = await _projectDataContext.GetByIdAsync(task.ProjectId.Id);
            }
        
            return task;
        }

        public async Task<Task?> CreateTask(string sprintId, Task task)
        {
            Task? result = null;
            var sprint = await _sprintDataContext.GetByIdAsync(new ObjectId(sprintId));
            if (sprint != null)
            {
                task.ProjectId = sprint.ProjectId;
                task.SprintIds.Add(new DocumentReference(nameof(Sprint),new ObjectId(sprintId)));
                var insertedTask = await _taskDataContext.AddAsync(task);
                sprint.TaskIds.Add(new (nameof(Task),insertedTask.Id));
                await _sprintDataContext.UpdateAsync(sprint);
            
                result = task;
            }

            return result;
        }
        
        public Task<bool> UpdateTask(string id, Task task)
        {
            task.Id = new ObjectId(id);
            return _taskDataContext.UpdateAsync(task);
        }

        public async Task<bool> DeleteTask(string id)
        {
            return await _taskDataContext.DeleteAsync(new ObjectId(id));
        }
    }
}
