using F1DriversApi.Models;

namespace F1DriversApi.Repositories;

public interface IDriverRepository
{
    Task<IEnumerable<Driver>> GetDriversBySessionAsync(int sessionKey);

    Task<Driver?> GetDriverByNumberAndSessionAsync(int driverNumber, int sessionKey);
    
    Task<bool> SessionHasDriversAsync(int sessionKey);
    
    Task<Driver> SaveDriverAsync(Driver driver);
    
    Task<int> SaveDriversAsync(IEnumerable<Driver> drivers);

}