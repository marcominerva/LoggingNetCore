using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfWeatherApp.Models;
using WpfWeatherApp.Settings;

namespace WpfWeatherApp
{
    public partial class MainWindow : Window
    {
        private readonly AppSettings settings;
        private readonly ILogger<MainWindow> logger;
        private readonly IHttpClientFactory httpClientFactory;

        public MainWindow(IOptions<AppSettings> settings, ILogger<MainWindow> logger, IHttpClientFactory httpClientFactory)
        {
            InitializeComponent();

            this.settings = settings.Value;
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ConditionCityTextBlock.Text = null;
            ConditionImage.Source = null;
            ConditionTextBlock.Text = null;
            TemperatureTextBlock.Text = null;

            logger.LogDebug("Getting weather condition for {ZipCode}, {Country}...", ZipCodeTextBox.Text, CountryTextBox.Text);

            var httpClient = httpClientFactory.CreateClient("openweathermap");
            using var weatherResponse = await httpClient.GetAsync($"weather?zip={ZipCodeTextBox.Text},{CountryTextBox.Text}&units=metric&APPID={settings.OpenWeatherMapApiKey}");

            if (weatherResponse.IsSuccessStatusCode)
            {
                logger.LogInformation("Weather condition for {ZipCode}, {Country} retrieved", ZipCodeTextBox.Text, CountryTextBox.Text);

                var responseContent = await weatherResponse.Content.ReadAsStringAsync();
                var weather = new WeatherInfo(responseContent);

                ConditionCityTextBlock.Text = weather.CityName;
                ConditionImage.Source = new BitmapImage(new Uri(weather.ConditionIconUrl));
                ConditionTextBlock.Text = weather.Condition;
                TemperatureTextBlock.Text = $"{weather.Temperature}°C";
            }
            else
            {
                logger.LogError("Unable to retrieve weather condition for {ZipCode}, {Country}. Error code: {ErrorCode}", ZipCodeTextBox.Text, CountryTextBox.Text, (int)weatherResponse.StatusCode);
                MessageBox.Show($"Unable to retrieve weather codition ({(int)weatherResponse.StatusCode}).", "Weather Client", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
