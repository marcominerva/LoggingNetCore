using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;
using System.Windows;
using WpfWeatherApp.Settings;

namespace WpfWeatherApp
{
    public partial class App : Application
    {
        private readonly IHost host;

        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(context, services);
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);

                    Serilog.Debugging.SelfLog.Enable(msg =>
                    {
                        Debug.Print(msg);
                    });
                })
                .Build();

            ServiceProvider = host.Services;
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var appSettingsSection = context.Configuration.GetSection(nameof(AppSettings));
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.Configure<AppSettings>(appSettingsSection);

            services.AddHttpClient("openweathermap").ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(appSettings.OpenWeatherMapUrl);
            });

            // Register all the Windows of the applications.
            services.AddSingleton<MainWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();

            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (host)
            {
                await host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}
