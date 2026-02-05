using F1DriversApi.Models;

namespace F1DriversApi.Repositories;

public class DriverRepository : IDriverRepository
{
    public Task<IEnumerable<Driver>> GetDriversBySessionAsync(int sessionKey)
    {
        throw new NotImplementedException();
    }

    public Task<Driver?> GetDriverByNumberAndSessionAsync(int driverNumber, int sessionKey)
    {
        throw new NotImplementedException();
    }
}