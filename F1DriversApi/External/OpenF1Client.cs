using F1DriversApi.Models;

namespace F1DriversApi.External;

public class OpenF1Client: IOpenF1Client
{
    public Task<IEnumerable<OpenF1DriverResponse>> GetDriversAsync(string sessionKey = "latest")
    {
        throw new NotImplementedException();
    }

    public Task<OpenF1DriverResponse?> GetDriverByNumberAsync(int driverNumber, string sessionKey = "latest")
    {
        throw new NotImplementedException();
    }

    public Task<int> GetLatestSessionKeyAsync()
    {
        throw new NotImplementedException();
    }
}