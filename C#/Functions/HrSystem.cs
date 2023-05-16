using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HrSystem.Functions;

public static class Request
{
    [FunctionName("RequestOrchestrator")]
    public static async Task<HrResponse> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context
    )
    {
        var request = context.GetInput<HrRequest>();

        var response = new HrResponse
        {
            HrRequest = request
        };

        return response;
    }

    [FunctionName("Approval")]
    public static HrResponse Approval(
        [HttpTrigger] string instanceId
    )
    {
        return new HrResponse();
    }

    [FunctionName("Request_Hello")]
    public static string SayHello([ActivityTrigger] string name, ILogger log)
    {
        log.LogInformation($"Saying hello to {name}.");
        return $"Hello {name}!";
    }

    [FunctionName("HrRequest")]
    public static async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        var body = await req.Content.ReadAsStringAsync();

        var hrRequest = JsonConvert.DeserializeObject<HrRequest>(body);

        var instanceId = await starter.StartNewAsync("RequestOrchestrator", hrRequest);

        log.LogInformation($"Started HR Request for '{hrRequest}' with ID = '{instanceId}'.");

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}