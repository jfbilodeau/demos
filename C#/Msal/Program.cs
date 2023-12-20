 using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

Console.WriteLine("Starting MSAL demo...");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var clientId = configuration["CLIENT_ID"];
var tenantId = configuration["TENANT_ID"];

var authority = $"https://login.microsoftonline.com/{tenantId}";

var publicClientApp = PublicClientApplicationBuilder.Create(clientId)
    .WithTenantId(tenantId)
    // .WithAuthority(authority)
    .WithDefaultRedirectUri()
    .Build();

var scopes = new[]
{
    "user.read",
    
};

var authResult = await publicClientApp.AcquireTokenInteractive(scopes).ExecuteAsync();
// var authResult = await publicClientApp.AcquireTokenWithDeviceCode(scopes, deviceCodeResult => {
//     Console.WriteLine(deviceCodeResult.Message);
//     return Task.FromResult(0);
// }).ExecuteAsync();

var account = authResult.Account;

Console.WriteLine($"Logged in as {account.Username}");

var claims = authResult.ClaimsPrincipal;

Console.WriteLine("Claims:");

foreach (var claim in claims.Claims) Console.WriteLine($" - {claim}");

var authScopes = authResult.Scopes;

Console.WriteLine("Scopes:");

foreach (var scope in authScopes) Console.WriteLine($" - {scope}");

Console.WriteLine("Done!");