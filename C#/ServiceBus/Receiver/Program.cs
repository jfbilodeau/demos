using System.Text.Json;
using Azure.Messaging.ServiceBus;
using HrSystem;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting ServiceBusReceiver...");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var connectionString = configuration["connectionString"];

await using var client = new ServiceBusClient(connectionString);

var receiver = client.CreateReceiver(
    "demotopic",
    "demosub",
    new ServiceBusReceiverOptions
    {
        // ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete
    }
);

var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(15)).Token;

var batch = await receiver.ReceiveMessagesAsync(10, cancellationToken: cancellationToken);

foreach (var message in batch)
{
    var json = message.Body.ToString();

    var employee = JsonSerializer.Deserialize<Employee>(json);

    Console.WriteLine($"Processing: {employee}");

    await receiver.CompleteMessageAsync(message);
}

Console.WriteLine("Done!");