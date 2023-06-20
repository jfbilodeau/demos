using System.Text.Json;
using Azure.Messaging.ServiceBus;
using HrSystem;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting ServiceBusSender...");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var connectionString = configuration["connectionString"];

await using var client = new ServiceBusClient(connectionString);

var sender = client.CreateSender("demotopic");

using var batch = await sender.CreateMessageBatchAsync();

var employees = Employees.List();

foreach (var employee in employees)
{
    var json = JsonSerializer.Serialize(employee);

    var message = new ServiceBusMessage(json);

    if (!batch.TryAddMessage(message)) throw new Exception($"Could not add message to batch: {message}");
}

await sender.SendMessagesAsync(batch);

Console.WriteLine("Done!");