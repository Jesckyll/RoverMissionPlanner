using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("rovers/{roverName}/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IRoverTaskService _service;

        public TasksController(IRoverTaskService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(string roverName, [FromBody] RoverTaskDto dto)
        {
            var task = new RoverTask
            {
                Id = Guid.NewGuid(),
                RoverName = roverName,
                TaskType = Enum.Parse<TaskType>(dto.TaskType),
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                StartsAt = dto.StartsAt,
                DurationMinutes = dto.DurationMinutes,
                Status = Domain.Entities.TaskStatus.Planned
            };

            var added = await _service.AddTaskAsync(task);
            if (!added)
                return Conflict("La tarea se solapa con una existente.");

            return CreatedAtAction(nameof(GetTasks), new { roverName, date = task.StartsAt.Date.ToString("yyyy-MM-dd") }, task);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(string roverName, [FromQuery] DateTime date)
        {
            var tasks = await _service.GetTasksByDateAsync(roverName, date);
            return Ok(tasks);
        }

        [HttpGet("~/rovers/{roverName}/utilization")]
        public async Task<IActionResult> GetUtilization(string roverName, [FromQuery] DateTime date)
        {
            var usage = await _service.GetUtilizationAsync(roverName, date);
            return Ok(new { utilizationPercent = usage });
        }
    }
}
