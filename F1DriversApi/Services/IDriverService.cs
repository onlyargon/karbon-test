using F1DriversApi.Models;

namespace F1DriversApi.Services;

public interface IDriverService
{
    Task<IEnumerable<Driver>> GetDriversAsync(int? sessionKey = null);
    
    Task<Driver?> GetDriverByNumberAsync(int driverNumber, int? sessionKey = null);
}