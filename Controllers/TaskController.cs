using TaskManagerAPI.Models;
using TaskManagerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public TaskController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpGet]
        public async Task<List<TaskItem>> Get() => await _mongoDbService.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetById(string id)
        {
            var task = await _mongoDbService.GetByIdAsync(id);
            if (task == null)
                return NotFound();
            return task;
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem newTask)
        {
            await _mongoDbService.CreateAsync(newTask);
            return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, TaskItem updatedTask)
        {
            var task = await _mongoDbService.GetByIdAsync(id);
            if (task == null)
                return NotFound();

            await _mongoDbService.UpdateAsync(id, updatedTask);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var task = await _mongoDbService.GetByIdAsync(id);
            if (task == null)
                return NotFound();

            await _mongoDbService.DeleteAsync(id);
            return NoContent();
        }
    }
}