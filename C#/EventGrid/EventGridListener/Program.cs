using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting EventGridListener");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var endpoint = "";
var key = "";

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