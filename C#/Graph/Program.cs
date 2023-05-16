using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

Console.WriteLine("Starting Microsoft Graph demo...");

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var clientId = configuration["CLIENT_ID"];
var tenantId = configuration["TENANT_ID"];

var options = new TokenCredentialOptions
{
    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
};
var scopes = new[]
{
    "user.read",
    "Calendars.Read.Shared"
};

// var pca = PublicClientApplicationBuilder
//     .Create(clientId)
//     .WithTenantId(tenantId)
//     .Build();
//
// var authProvider = new DelegateAuthenticationProvider(async (request) => {
//     var result = await pca.AcquireTokenByIntegratedWindowsAuth(scopes).ExecuteAsync();
//
//     request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
// });
//
// var graphClient = new GraphServiceClient(authProvider);

Console.WriteLine("Getting device code credential...");

Task Callback(DeviceCodeInfo code, CancellationToken cancellation)
{
    Console.WriteLine("------------------------------------------------------------");
    Console.WriteLine($"User code: {code.UserCode}");
    Console.WriteLine($"Message: {code.Message}");
    Console.WriteLine("------------------------------------------------------------");

    return Task.FromResult(0);
}

var deviceCodeCredential = new DeviceCodeCredential(Callback, tenantId, clientId, options);

var graphClient = new GraphServiceClient(deviceCodeCredential, scopes);

Console.WriteLine("Getting profile picture...");
var photoStream = await graphClient.Me.Photo.Content.Request().GetAsync();

var fileStream = new FileStream("Photo.png", FileMode.Create, FileAccess.Write);

await photoStream.CopyToAsync(fileStream);

// Console.WriteLine("Finding available time for meeting...");
// var times = await graphClient
//     .Me
//     .FindMeetingTimes(meetingDuration: new Duration(TimeSpan.FromMinutes(30)))
//     .Request()
//     .PostAsync();
//
// Console.WriteLine("Times available:");
//
// foreach (var time in times.MeetingTimeSuggestions)
// {
//     Console.WriteLine($" - {time}");
// }

// var messages = await graphClient.Me.Messages
//     .Request()
//     .OrderBy("receivedDateTime")
//     .GetAsync();

Console.WriteLine("Done!");