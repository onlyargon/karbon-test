namespace F1DriversApi.Models;

public class Driver
{
    public int Id { get; set; }
    public int DriverNumber { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
    public string SessionKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}