using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

Console.WriteLine("Starting Demo...");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var channel = new InMemoryChannel();

try
{
    var services = new ServiceCollection();

    services.Configure<TelemetryConfiguration>(config => config.TelemetryChannel = channel);
    services.AddLogging(builder =>
    {
        var connectionString = configuration["connectionString"];

        builder.AddApplicationInsights(
            config => { config.ConnectionString = connectionString; },
            options => { }
        );
    });

    // TelemetryConfiguration.Active.TelemetryInitializers.Add(new AppTelemetryInitializer());

    var serviceProvider = services.BuildServiceProvider();
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Hello from Logger");
    logger.LogWarning("It is now {Now}", DateTime.Now);

    var scopeName = $"Demo Scope {Guid.NewGuid()}";
    Console.WriteLine($"Entering scope: {scopeName}");
    using (logger.BeginScope(scopeName))
    {
        logger.LogInformation("This message is in scope");
        logger.LogInformation("This message is also in scope");
    }

    Console.WriteLine($"Exiting scope: {scopeName}");

    logger.LogError("Oups! We got an error!");
}
finally
{
    channel.Flush();

    await Task.Delay(TimeSpan.FromMilliseconds(1000));
}

Console.WriteLine("Done!");