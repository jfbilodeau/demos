using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting Block Blob Demo...");

var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .AddUserSecrets<Program>()
    .Build();

var connectionString = configuration["connectionString"];
var containerName = "private";

Console.WriteLine("Creating Container...");
var container = new BlobContainerClient(connectionString, containerName);
await container.CreateIfNotExistsAsync();

Console.WriteLine("Deleting all blobs...");
{
    var list = container.GetBlobsAsync();

    await foreach (var item in list)
    {
        Console.WriteLine($"Deleting {item.Name}...");
        await container.DeleteBlobAsync(item.Name);
    }
}

Console.WriteLine("Uploading block blob...");
{
    var blobName = "block.txt";
    var blob = container.GetBlobClient(blobName);
    await blob.DeleteIfExistsAsync();

    await using var stream = File.Open("../../Data/Employees.json", FileMode.Open);

    await container.UploadBlobAsync(blobName, stream);
}

Console.WriteLine("Uploading page blob");
{
    var blobName = "page.txt";
    var message = "This is a page blob...";
    var bytes = Encoding.UTF8.GetBytes(message);
    var blob = container.GetPageBlobClient(blobName);
    Console.WriteLine($"Page size: {blob.PageBlobPageBytes}");

    blob.CreateIfNotExists(blob.PageBlobPageBytes);

    var buffer = new byte[blob.PageBlobPageBytes];
    bytes.CopyTo(buffer, 0);
    using var stream = new MemoryStream(buffer);

    Console.WriteLine(buffer.Length);

    await blob.UploadPagesAsync(stream, 0);
}

Console.WriteLine("Writing to append blob");
{
    var blobName = "append.txt";
    var log = container.GetAppendBlobClient(blobName);

    await log.CreateIfNotExistsAsync(new AppendBlobCreateOptions());

    var message1 = new MemoryStream(Encoding.UTF8.GetBytes("First message"));
    var message2 = new MemoryStream(Encoding.UTF8.GetBytes("Second message"));

    log.AppendBlock(message1);
    log.AppendBlock(message2);
}

Console.WriteLine("Listing blobs...");
{
    var list = container.GetBlobsAsync();

    await foreach (var item in list) {
        Console.WriteLine($"Found {item.Name}");
    }
}

Console.WriteLine("Done!");