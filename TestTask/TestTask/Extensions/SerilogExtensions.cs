using Serilog;
using Serilog.Events;

namespace TestTask.Extensions
{
    public static class SerilogExtensions
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            var loggerConfig = new LoggerConfiguration()
                .WriteTo.Console() // Log output to the console.
                .WriteTo.File(
                    "Logs/log.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

            // Adjust the overall minimum log level based on the environment.
            if (builder.Environment.IsDevelopment())
            {
                // In development mode, log more details (Debug level).
                loggerConfig = loggerConfig.MinimumLevel.Debug();
            }
            else
            {
                // In non-development environments, use a higher minimum level (Warning).
                loggerConfig = loggerConfig.MinimumLevel.Warning();
            }

            // Create the logger from the configuration.
            var logger = loggerConfig.CreateLogger();

            // Clear any existing logging providers.
            builder.Logging.ClearProviders();

            // Add Serilog as the logging provider.
            builder.Logging.AddSerilog(logger);

            // Configure the host to use Serilog.
            builder.Host.UseSerilog(logger);

            // Register the Serilog logger instance as a singleton service,
            // so that it can be injected into other parts of the application.
            builder.Services.AddSingleton<Serilog.ILogger>(logger);
        }
    }
}
