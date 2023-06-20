using System.Text.Json;
using Azure.Storage.Queues;
using HrSystem;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting Queue Storage Update...");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var connectionString = configuration["connectionString"];

var client = new QueueClient(connectionString, "demoqueue");

await client.CreateAsync();

var messages = await client.ReceiveMessagesAsync(10, TimeSpan.FromSeconds(20));

Console.WriteLine($"Received {messages.Value.Length} messages.");

foreach (var message in messages.Value)
{
    var employee = JsonSerializer.Deserialize<Employee>(message.MessageText);

    if (employee.Salary < 70000)
    {
        Console.WriteLine($"Updating salary of {employee.FirstName} {employee.LastName}");

        employee.Salary = 70000;

        var updatedMessage = JsonSerializer.Serialize(employee);

        await client.UpdateMessageAsync(message.MessageId, message.PopReceipt, updatedMessage, TimeSpan.FromSeconds(5));
    }
}

Console.WriteLine("Done!");