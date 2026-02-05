using System.Text.Json;
using F1DriversApi.Models;

namespace F1DriversApi.External;

public class OpenF1Client(HttpClient client) : IOpenF1Client
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<IEnumerable<OpenF1DriverResponse>> GetDriversAsync(string sessionKey = "latest")
    {
        var url = $"drivers?session_key={sessionKey}";

        try
        {
            var res = await client.GetAsync(url);
            res.EnsureSuccessStatusCode();
            
            var drivers = await res.Content.ReadFromJsonAsync<List<OpenF1DriverResponse>>(_jsonOptions);

            if (drivers is null || drivers.Count == 0)
            {
                return Array.Empty<OpenF1DriverResponse>();
            }

            return drivers;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<OpenF1DriverResponse?> GetDriverByNumberAsync(int driverNumber, string sessionKey = "latest")
    {
        var url = $"drivers?driver_number={driverNumber}&session_key={sessionKey}";

        try
        {
            var res = await client.GetAsync(url);
            res.EnsureSuccessStatusCode();
            
            var driver = await res.Content.ReadFromJsonAsync<List<OpenF1DriverResponse>>(_jsonOptions);

            if (driver is null || driver.Count == 0)
            {
                return null;
            }
            
            return driver.FirstOrDefault();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<int> GetLatestSessionKeyAsync()
    {
        var drivers = await GetDriversAsync();
        var firstDriver = drivers.FirstOrDefault();

        if (firstDriver is null)
        {
            return 0;
        }
        
        return firstDriver.SessionKey;
    }
}