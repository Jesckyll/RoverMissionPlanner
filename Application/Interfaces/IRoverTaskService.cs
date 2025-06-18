using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRoverTaskService
    {
        Task<bool> AddTaskAsync(RoverTask task);
        Task<IEnumerable<RoverTask>> GetTasksByDateAsync(string roverName, DateTime date);
        Task<double> GetUtilizationAsync(string roverName, DateTime date);
    }
}