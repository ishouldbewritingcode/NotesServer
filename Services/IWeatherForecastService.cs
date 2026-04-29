using NotesServer.Models;

namespace NotesServer.Services;

public interface IWeatherForecastService
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecasts();
}
