namespace Application.DTOs;

public class RoverTaskDto
{
    public string RoverName { get; set; } = string.Empty;
    public string TaskType { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime StartsAt { get; set; }
    public int DurationMinutes { get; set; }
}
