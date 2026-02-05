using F1DriversApi.Models;

namespace F1DriversApi.Services;

public class DriverService : IDriverService
{
    public Task<IEnumerable<Driver>> GetDriversAsync(int? sessionKey = null)
    {
        throw new NotImplementedException();
    }

    public Task<Driver?> GetDriverByNumberAsync(int driverNumber, int? sessionKey = null)
    {
        throw new NotImplementedException();
    }
}