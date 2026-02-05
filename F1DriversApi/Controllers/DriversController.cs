using F1DriversApi.Models;
using F1DriversApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace F1DriversApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController(DriverService service) : ControllerBase
{
    // Get Drivers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Driver>>> GetDriversAsync(int? sessionKey = null)
    {
        try
        {
            var drivers = await service.GetDriversAsync(sessionKey);
            return Ok(drivers);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // GetDriverByDriverId
    [HttpGet("{driverNumber:int}")]
    public async Task<ActionResult<Driver?>> GetDriverByNumberAsync(int driverNumber, int? sessionKey = null)
    {
        try
        {
            if (driverNumber < 1 || driverNumber > 99)
            {
                return BadRequest("Driver number must be between 1 and 99");
            }
            
            var driver = await service.GetDriverByNumberAsync(driverNumber, sessionKey);
            if (driver == null)
            {
                return NotFound();
            }
            
            return Ok(driver);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}