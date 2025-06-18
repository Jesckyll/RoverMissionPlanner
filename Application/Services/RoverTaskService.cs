using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class RoverTaskService : IRoverTaskService
    {
        private readonly List<RoverTask> _tasks = new();

        public async Task<bool> AddTaskAsync(RoverTask newTask)
        {
            // Regla: No se puede solapar con otras tareas del mismo rover
            var overlapping = _tasks.Any(t =>
                t.RoverName == newTask.RoverName &&
                t.StartsAt < newTask.StartsAt.AddMinutes(newTask.DurationMinutes) &&
                newTask.StartsAt < t.StartsAt.AddMinutes(t.DurationMinutes));

            if (overlapping)
                return false;

            _tasks.Add(newTask);
            return true;
        }

        public async Task<IEnumerable<RoverTask>> GetTasksByDateAsync(string roverName, DateTime date)
        {
            return _tasks
                .Where(t =>
                    t.RoverName.Equals(roverName, StringComparison.OrdinalIgnoreCase) &&
                    t.StartsAt.Date == date.Date)
                .OrderBy(t => t.StartsAt);
        }

        public async Task<double> GetUtilizationAsync(string roverName, DateTime date)
        {
            var tasks = await GetTasksByDateAsync(roverName, date);
            var totalMinutes = tasks.Sum(t => t.DurationMinutes);
            return (double)totalMinutes / 1440.0 * 100.0; // % del día usado
        }
    }
}
