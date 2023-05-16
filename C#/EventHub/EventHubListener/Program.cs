﻿using Azure.Messaging.EventHubs;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting Event Hub Listener...");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var storageAccountConnectionString = configuration["storageAccountConnectionString"];

var checkPoint = new BlobContainerClient(storageAccountConnectionString, "process");

var connectionString = configuration["connectionString"];

var eventHubName = "employee";

var group = "updatehrsystem";

var processor = new EventProcessorClient(
    checkPoint,
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