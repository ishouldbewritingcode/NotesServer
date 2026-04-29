using GraphQL.Types;
using NotesServer.Models;

namespace NotesServer.GraphQL.Types;

public class WeatherForecastType : ObjectGraphType<WeatherForecast>
{
    public WeatherForecastType()
    {
        Field(x => x.Date).Description("The date of the forecast");
        Field(x => x.TemperatureC).Description("Temperature in Celsius");
        Field(x => x.TemperatureF).Description("Temperature in Fahrenheit");
        Field(x => x.Summary).Description("Weather summary");
    }
}
