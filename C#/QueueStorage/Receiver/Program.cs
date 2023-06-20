using System.Text.Json;
using Azure.Storage.Queues;
using HrSystem;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting Queue Storage Receiver...");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var connectionString = configuration["connectionString"];

var client = new QueueClient(connectionString, "demoqueue");

await client.CreateAsync();

var messages = await client.ReceiveMessagesAsync(10, TimeSpan.FromSeconds(20));

Console.WriteLine($"Received {messages.Value.Length} messages.");

foreach (var message in messages.Value)
{
    var employee = JsonSerializer.Deserialize<Employee>(message.MessageText);

    Console.WriteLine($"Processed {employee}");

    await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);
}

Console.WriteLine("Done!");