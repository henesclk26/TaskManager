using TaskManagerAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace TaskManagerAPI.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<TaskItem> _tasksCollection;

        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _tasksCollection = database.GetCollection<TaskItem>(mongoDbSettings.Value.CollectionName);
        }

        public async Task<List<TaskItem>> GetAllAsync() =>
            await _tasksCollection.Find(_ => true).ToListAsync();

        public async Task<TaskItem> GetByIdAsync(string id) =>
            await _tasksCollection.Find(t => t.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(TaskItem task) =>
            await _tasksCollection.InsertOneAsync(task);

        public async Task UpdateAsync(string id, TaskItem updatedTask) =>
            await _tasksCollection.ReplaceOneAsync(t => t.Id == id, updatedTask);

        public async Task DeleteAsync(string id) =>
            await _tasksCollection.DeleteOneAsync(t => t.Id == id);
    }
}