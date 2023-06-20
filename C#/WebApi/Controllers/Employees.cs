using HrSystem;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly HrContext hrContext;
    private readonly ILogger<EmployeesController> logger;

    public EmployeesController(ILogger<EmployeesController> logger, HrContext hrContext)
    {
        this.logger = logger;
        this.hrContext = hrContext;
    }

    [HttpGet(Name = "list")]
    public IEnumerable<Employee> Get()
    {
        logger.LogInformation("Getting all employees");
        
        return hrContext.Employees.ToList();
    }
}