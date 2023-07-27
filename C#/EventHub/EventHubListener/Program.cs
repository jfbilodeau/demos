using Azure.Messaging.EventHubs;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting Event Hub Listener...");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var storageAccountConnectionString = configuration["storageAccountConnectionString"];

var storageAccount = new BlobContainerClient(storageAccountConnectionString, "process");

var connectionString = configuration["connectionString"];

var eventHubName = "demohub";

var group = "demosub";

var processor = new EventProcessorClient(
    storageAccount,
    group,
    connectionString,
    eventHubName
);

processor.ProcessEventAsync += async receivedEvent =>
{
    var json = receivedEvent.Data.EventBody.ToString();

    Console.WriteLine($"Received: {json}");

    await receivedEvent.UpdateCheckpointAsync();
};

processor.ProcessErrorAsync += error =>
{
    Console.Error.WriteLine($"Error occured: ${error}");

    return Task.CompletedTask;
};

await processor.StartProcessingAsync();

await Task.Delay(TimeSpan.FromSeconds(10));

await processor.StopProcessingAsync();

Console.WriteLine("Done!");