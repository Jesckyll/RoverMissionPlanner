using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Xunit;

public class OverlapTests
{
    private readonly IRoverTaskService _service;

    public OverlapTests()
    {
        _service = new RoverTaskService();
    }

    [Fact]
    public async Task AddTask_ShouldSucceed_WhenNoOverlap()
    {
        var task = new RoverTask
        {
            Id = Guid.NewGuid(),
            RoverName = "Curiosity",
            TaskType = TaskType.Drill,
            Latitude = 0,
            Longitude = 0,
            StartsAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
            DurationMinutes = 60
        };

        var result = await _service.AddTaskAsync(task);

        Assert.True(result);
    }

    [Fact]
    public async Task AddTask_ShouldFail_WhenOverlapping()
    {
        var firstTask = new RoverTask
        {
            Id = Guid.NewGuid(),
            RoverName = "Curiosity",
            TaskType = TaskType.Drill,
            Latitude = 0,
            Longitude = 0,
            StartsAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
            DurationMinutes = 60
        };

        var secondTask = new RoverTask
        {
            Id = Guid.NewGuid(),
            RoverName = "Curiosity",
            TaskType = TaskType.Sample,
            Latitude = 0,
            Longitude = 0,
            StartsAt = new DateTime(2025, 1, 1, 10, 30, 0, DateTimeKind.Utc),
            DurationMinutes = 30
        };

        await _service.AddTaskAsync(firstTask);
        var result = await _service.AddTaskAsync(secondTask);

        Assert.False(result);
    }

    [Fact]
    public async Task AddTask_ShouldSucceed_WhenDifferentRovers()
    {
        var task1 = new RoverTask
        {
            Id = Guid.NewGuid(),
            RoverName = "Curiosity",
            TaskType = TaskType.Photo,
            Latitude = 0,
            Longitude = 0,
            StartsAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc),
            DurationMinutes = 30
        };

        var task2 = new RoverTask
        {
            Id = Guid.NewGuid(),
            RoverName = "Perseverance",
            TaskType = TaskType.Photo,
            Latitude = 0,
            Longitude = 0,
            StartsAt = task1.StartsAt,
            DurationMinutes = task1.DurationMinutes
        };

        await _service.AddTaskAsync(task1);
        var result = await _service.AddTaskAsync(task2);

        Assert.True(result);
    }
}
