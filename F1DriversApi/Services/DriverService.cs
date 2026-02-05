using F1DriversApi.External;
using F1DriversApi.Models;
using F1DriversApi.Repositories;

namespace F1DriversApi.Services;

public class DriverService(IOpenF1Client openF1Client, IDriverRepository driverRepository) : IDriverService
{
    public async Task<IEnumerable<Driver>> GetDriversAsync(int? sessionKey = null)
    {
        var session = sessionKey ?? await GetSessionKeyAsync();
        
        // check in db for cached drivers data
        if (await driverRepository.SessionHasDriversAsync(session))
        {
            return await driverRepository.GetDriversBySessionAsync(session);
        }
        
        // fetch from api if not
        var apiDrivers = await openF1Client.GetDriversAsync(session.ToString());
        if (!apiDrivers.Any())
        {
            return new List<Driver>();
        }
        
        // save fetched data to db
        var drivers = apiDrivers.Select(d => d.ToDriver()).ToList();
        await driverRepository.SaveDriversAsync(drivers);
        
        // get data from db and return
        return await driverRepository.GetDriversBySessionAsync(session);
    }

    public async Task<Driver?> GetDriverByNumberAsync(int driverNumber, int? sessionKey = null)
    {
        var session = sessionKey ?? await GetSessionKeyAsync();
        
        // check in db for cached driver
        var cachedDriver = await driverRepository.GetDriverByNumberAndSessionAsync(driverNumber, session);
        if (cachedDriver != null)
        {
            return cachedDriver;
        }
        
        // check that driver is in the session
        if (await driverRepository.SessionHasDriversAsync(session))
        {
            return null;
        }
        
        // fetch from api
        var apiDriver = await openF1Client.GetDriverByNumberAsync(driverNumber,session.ToString());
        if (apiDriver is null)
        {
            return null;
        }
        
        // save to db
        var driver = apiDriver.ToDriver();
        var savedDriver = await driverRepository.SaveDriverAsync(driver);
        
        return savedDriver;
    }

    private async Task<int> GetSessionKeyAsync()
    {
        return await openF1Client.GetLatestSessionKeyAsync();
    }
}