using System.Text.Json;
using Azure.Storage.Queues;
using HrSystem;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting Queue Storage Sender...");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var connectionString = configuration["connectionString"];

var client = new QueueClient(connectionString, "demoqueue");

await client.CreateAsync();

var employees = Employees.List();

foreach (var employee in employees)
{
    var json = JsonSerializer.Serialize(employee);

    Console.WriteLine($"Adding: {json}");

    await client.SendMessageAsync(json, timeToLive: TimeSpan.FromHours(1));
}

Console.WriteLine("Done!");