using HrSystem;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting demo...");

var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .AddUserSecrets<Program>()
    .Build();

var uri = configuration["uri"];
var key = configuration["key"];

var client = new CosmosClient(
    uri,
    key,
    new CosmosClientOptions
    {
        SerializerOptions = new CosmosSerializationOptions
        {
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
        }
    }
);

var container = client.GetContainer("hr", "employees");

var id = "1004";
var partitionKey = new PartitionKey(id);

var newEmployee = new Employee
{
    Id = id,
    FirstName = "Allan",
    LastName = "Turing",
    Salary = 70000
};

// await container.DeleteItemAsync<Employee>(
//     id, 
//     partitionKey
// );

await container.CreateItemAsync(newEmployee, partitionKey);

var query = new QueryDefinition("SELECT * FROM e where e.firstName='Ada'");

var employees = container.GetItemQueryIterator<Employee>(query);

while (employees.HasMoreResults)
{
    var batch = await employees.ReadNextAsync();

    foreach (var item in batch) 
    {
        Console.WriteLine($"ID: {item.Id}, lastName: {item.LastName}");
    }
}

Console.WriteLine("Done!");