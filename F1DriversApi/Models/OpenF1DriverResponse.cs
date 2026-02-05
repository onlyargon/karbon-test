using System.Text.Json.Serialization;

namespace F1DriversApi.Models;

public class OpenF1DriverResponse
{
    [JsonPropertyName("driver_number")]
    public int DriverNumber { get; set; }
    
    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }
    
    [JsonPropertyName("team_name")]
    public string? TeamName { get; set; }
    
    [JsonPropertyName("session_key")]
    public int SessionKey { get; set; }
    
    public Driver ToDriver()
    {
        return new Driver
        {
            DriverNumber = DriverNumber,
            FullName = FullName,
            TeamName = TeamName,
            SessionKey = SessionKey,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}