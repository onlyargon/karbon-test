using F1DriversApi.External;
using F1DriversApi.Models;
using F1DriversApi.Repositories;

namespace F1DriversApi.Services;

public class DriverService(IOpenF1Client openF1Client, IDriverRepository driverRepository) : IDriverService
{
    public async Task<IEnumerable<Driver>> GetDriversAsync(int? sessionKey = null)
    {
        var _sessionKey = sessionKey ?? await GetSessionKeyAsync();
        
        // check in db for cached drivers data
        
        // fetch from api if not
        var apiDrivers = await openF1Client.GetDriversAsync(_sessionKey.ToString());
        if (!apiDrivers.Any())
        {
            return new List<Driver>();
        }
        
        // save fetched data to db
        
        
        // get data from db and return
        return await driverRepository.GetDriversBySessionAsync(_sessionKey);
    }

    public async Task<Driver?> GetDriverByNumberAsync(int driverNumber, int? sessionKey = null)
    {
        var _sessionKey = sessionKey ?? await GetSessionKeyAsync();
        
        // check in db for cached driver
        
        // check that driver is in the session
        
        // fetch from api
        
        // save to db

        return null;
    }

    private async Task<int> GetSessionKeyAsync()
    {
        return await openF1Client.GetLatestSessionKeyAsync();
    }
}