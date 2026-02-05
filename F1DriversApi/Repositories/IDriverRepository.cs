using F1DriversApi.Models;

namespace F1DriversApi.Repositories;

public interface IDriverRepository
{
    Task<IEnumerable<Driver>> GetDriversBySessionAsync(int sessionKey);

    Task<Driver?> GetDriverByNumberAndSessionAsync(int driverNumber, int sessionKey);

}