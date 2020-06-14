using System.Text.Json;

namespace WpfWeatherApp.Models
{
    public class WeatherInfo
    {
        public string CityName { get; }

        public string ConditionIcon { get; }

        public string ConditionIconUrl => $"https://openweathermap.org/img/w/{ConditionIcon}.png";

        public string Condition { get; }

        public double Temperature { get; }

        public WeatherInfo(string cityName, string condition, string conditionIcon, double temperature)
            => (CityName, ConditionIcon, Condition, Temperature) = (cityName, conditionIcon, condition, temperature);

        public WeatherInfo(string openWeatherMapResponse)
        {
            using var jsonDocument = JsonDocument.Parse(openWeatherMapResponse);
            var jsonRootElement = jsonDocument.RootElement;
            var weatherElement = jsonRootElement.GetProperty("weather")[0];

            CityName = jsonRootElement.GetProperty("name").ToString();
            Condition = weatherElement.GetProperty("description").ToString();
            ConditionIcon = weatherElement.GetProperty("icon").ToString();
            Temperature = jsonRootElement.GetProperty("main").GetProperty("temp").GetDouble();
        }
    }
}
