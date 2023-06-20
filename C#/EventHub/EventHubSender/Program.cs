using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using EventHubSender;
using HrSystem;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting Event Hub Sender");

var employees = new[]
{
    new NewEmployeeEvent
    {
        Employee = new Employee
        {
            Id = "123",
            FirstName = "Charles",
            LastName = "Babbage",
            Salary = 65000.00
        }
    },
    new NewEmployeeEvent
    {
        Employee = new Employee
        {
            Id = "456",
            FirstName = "Ada",
            LastName = "Lovelace",
            Salary = 73000.00
        }
    },
    new NewEmployeeEvent
    {
        Employee = new Employee
        {
            Id = "789",
            FirstName = "Kathleen",
            LastName = "Booth",
            Salary = 65000.00
        }
    }
};

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var connectionString = configuration["connectionString"];

var eventHubName = "demohub";

var producerClient = new EventHubProducerClient(connectionString, eventHubName);

using var batch = await producerClient.CreateBatchAsync(new CreateBatchOptions
{
    PartitionKey = Guid.NewGuid().ToString()
});

foreach (var employee in employees)
{
    var json = JsonSerializer.Serialize(employee);

    var data = new EventData(json);

    Console.WriteLine($"Sending: {json}");

    if (!batch.TryAdd(data)) throw new Exception($"Could not add message to batch: {json}");
}

await producerClient.SendAsync(batch);

Console.WriteLine("Done!");