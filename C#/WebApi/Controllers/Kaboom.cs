using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KaboomController : ControllerBase
{
    private readonly ILogger<KaboomController> _logger;

    public KaboomController(ILogger<KaboomController> logger)
    {
        this._logger = logger;
    }

    [HttpGet(Name = "list")]
    public IEnumerable<object> Get()
    {
        throw new NotImplementedException("Something went wrong...");
    }
}