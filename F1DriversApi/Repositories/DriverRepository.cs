using F1DriversApi.Models;
using Microsoft.Data.SqlClient;

namespace F1DriversApi.Repositories;

public class DriverRepository(IConfiguration configuration) : IDriverRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                                throw new InvalidOperationException();
    public async Task<IEnumerable<Driver>> GetDriversBySessionAsync(int sessionKey)
    {
        var drivers = new List<Driver>();

        const string sql = @"
            SELECT Id, DriverNumber, FullName, TeamName, SessionKey,
                   CreatedAt, UpdatedAt
            FROM Drivers
            WHERE SessionKey = @SessionKey
            ORDER BY DriverNumber";

        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@SessionKey", sessionKey);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                drivers.Add(MapReaderToDriver(reader));
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        return drivers;
    }

    public async Task<Driver?> GetDriverByNumberAndSessionAsync(int driverNumber, int sessionKey)
    {
        const string sql = @"
            SELECT Id, DriverNumber, FullName, TeamName, SessionKey,
                   CreatedAt, UpdatedAt
            FROM Drivers
            WHERE DriverNumber = @DriverNumber AND SessionKey = @SessionKey";

        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DriverNumber", driverNumber);
            command.Parameters.AddWithValue("@SessionKey", sessionKey);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var driver = MapReaderToDriver(reader);
                return driver;
            }
            
            return null;
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<bool> SessionHasDriversAsync(int sessionKey)
    {
        const string sql = "SELECT COUNT(1) FROM Drivers WHERE SessionKey = @SessionKey";

        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@SessionKey", sessionKey);

            var count = (int)(await command.ExecuteScalarAsync() ?? 0);
            return count > 0;
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<Driver> SaveDriverAsync(Driver driver)
    {
        const string sql = @"
            MERGE INTO Drivers AS target
            USING (SELECT @DriverNumber AS DriverNumber, @SessionKey AS SessionKey) AS source
            ON target.DriverNumber = source.DriverNumber AND target.SessionKey = source.SessionKey
            WHEN MATCHED THEN
                UPDATE SET                     
                    FullName = @FullName,                   
                    TeamName = @TeamName,                    
                    UpdatedAt = GETUTCDATE()
            WHEN NOT MATCHED THEN
                INSERT (DriverNumber, FullName, TeamName, SessionKey)
                VALUES (@DriverNumber, @FullName,
                        @TeamName, @SessionKey)
            OUTPUT INSERTED.Id, INSERTED.CreatedAt, INSERTED.UpdatedAt;";

        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            AddDriverParameters(command, driver);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                driver.Id = reader.GetInt32(0);
                driver.CreatedAt = reader.GetDateTime(1);
                driver.UpdatedAt = reader.GetDateTime(2);
            }
            
            return driver;
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<int> SaveDriversAsync(IEnumerable<Driver> drivers)
    {
        var driverList = drivers.ToList();
        if (driverList.Count == 0) return 0;

        var savedCount = 0;

        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // Use a transaction for batch insert
            await using var transaction = connection.BeginTransaction();

            try
            {
                foreach (var driver in driverList)
                {
                    const string sql = @"
                        MERGE INTO Drivers AS target
                        USING (SELECT @DriverNumber AS DriverNumber, @SessionKey AS SessionKey) AS source
                        ON target.DriverNumber = source.DriverNumber AND target.SessionKey = source.SessionKey
                        WHEN MATCHED THEN
                            UPDATE SET                               
                                FullName = @FullName,  
                                TeamName = @TeamName,
                                UpdatedAt = GETUTCDATE()
                        WHEN NOT MATCHED THEN
                            INSERT (DriverNumber, FullName,
                                    TeamName, SessionKey)
                            VALUES (@DriverNumber, @FullName,
                                    @TeamName, @SessionKey);";

                    await using var command = new SqlCommand(sql, connection, transaction);
                    AddDriverParameters(command, driver);

                    await command.ExecuteNonQueryAsync();
                    savedCount++;
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        return savedCount;
    }

    private static Driver MapReaderToDriver(SqlDataReader reader)
    {
        return new Driver
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            DriverNumber = reader.GetInt32(reader.GetOrdinal("DriverNumber")),
            FullName = reader.IsDBNull(reader.GetOrdinal("FullName"))
                ? null : reader.GetString(reader.GetOrdinal("FullName")),
            TeamName = reader.IsDBNull(reader.GetOrdinal("TeamName")) 
                ? null : reader.GetString(reader.GetOrdinal("TeamName")),
            SessionKey = reader.GetInt32(reader.GetOrdinal("SessionKey")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
        };
    }
    
    private static void AddDriverParameters(SqlCommand command, Driver driver)
    {
        command.Parameters.AddWithValue("@DriverNumber", driver.DriverNumber);
        command.Parameters.AddWithValue("@FullName", (object?)driver.FullName ?? DBNull.Value);
        command.Parameters.AddWithValue("@TeamName", (object?)driver.TeamName ?? DBNull.Value);
        command.Parameters.AddWithValue("@SessionKey", driver.SessionKey);
    }
}