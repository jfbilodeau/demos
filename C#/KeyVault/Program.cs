using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting Key Vault Demo");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var uri = configuration["KEY_VAULT_URI"];

if (uri == null)
{
    Console.Error.WriteLine("Configuration 'KEY_VAULT_URI' not set. Aborting");
    return;
}

var client = new SecretClient(new Uri(uri), new DefaultAzureCredential());

await client.SetSecretAsync("secret", "This is a secret value!");

var secret = await client.GetSecretAsync("secret");

Console.WriteLine($"secret: {secret.Value.Value}");

Console.Error.WriteLine("Attempting to delete secret...");
var deleteSecretOperation = await client.StartDeleteSecretAsync("secret");
await deleteSecretOperation.WaitForCompletionAsync();
Console.WriteLine("Secret deleted!");

try
{
    Console.WriteLine("Attempting purge...");

    await client.PurgeDeletedSecretAsync("secret");
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Could not purge key. Reason: {exception.Message}");
}

try
{
    var deleteSecret = await client.GetSecretAsync("secret");

    Console.WriteLine($"deleteSecretValue: {deleteSecret.Value.Value}");
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Error: Could not get secret value. Reason: {exception.Message}");
}

Console.WriteLine("Done!");