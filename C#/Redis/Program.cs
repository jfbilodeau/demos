using System.Text.Json;
using HrSystem;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

Console.WriteLine("Connecting to Redis database...");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var connectionString = configuration["connectionString"];

await using var connection = await ConnectionMultiplexer.ConnectAsync(connectionString);

var database = connection.GetDatabase();

Console.WriteLine("Pinging database...");
var latency = database.Ping();
Console.WriteLine($"Ping Latency: {latency}");

var employee = Employees.List().First();

var json = JsonSerializer.Serialize(employee);

var keyName = $"projectName.appName.employee-{employee.Id}";

database.StringSet(keyName, json, TimeSpan.FromSeconds(3));

var value = database.StringGet(keyName);

Console.WriteLine($"Value: {value}");

Thread.Sleep(TimeSpan.FromSeconds(5));

value = database.StringGet(keyName);

Console.WriteLine($"Value: {value}");

Console.WriteLine("Done!");