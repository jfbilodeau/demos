using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting demo...");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var endpoint = configuration["endpoint"];
var key = configuration["key"];

var client = new EventGridPublisherClient(
    new Uri(endpoint),
    new AzureKeyCredential(key)
);

var gridEvent = new EventGridEvent(
    "Example.Subject",
    "Example.EventType",
    "1.0",
    "This is my event!"
);

await client.SendEventAsync(gridEvent);

Console.WriteLine("Done!");