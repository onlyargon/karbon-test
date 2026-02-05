using F1DriversApi.Models;

namespace F1DriversApi.External;

public interface IOpenF1Client
{
    Task<IEnumerable<OpenF1DriverResponse>> GetDriversAsync(string sessionKey = "latest");
    Task<OpenF1DriverResponse?> GetDriverByNumberAsync(int driverNumber, string sessionKey = "latest");
    Task<int> GetLatestSessionKeyAsync();
}