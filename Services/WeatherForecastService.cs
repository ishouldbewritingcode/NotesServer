using Microsoft.Data.Sqlite;
using NotesServer.Models;

namespace NotesServer.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly string _connectionString;

    public WeatherForecastService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=ReactTanstackNotes.db";
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts()
    {
        var forecasts = new List<WeatherForecast>();

        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            // Adjust this query based on your actual table schema
            command.CommandText = @"
                SELECT date, temperatureC, summary
                FROM WeatherForecasts
                LIMIT 5";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var forecast = new WeatherForecast(
                    DateOnly.Parse(reader.GetString(0)),
                    reader.GetInt32(1),
                    reader.IsDBNull(2) ? null : reader.GetString(2)
                );
                forecasts.Add(forecast);
            }
        }
        catch (Exception ex)
        {
            // Log the exception or return empty list
            Console.WriteLine($"Error fetching weather forecasts: {ex.Message}");
        }

        return forecasts;
    }
}
