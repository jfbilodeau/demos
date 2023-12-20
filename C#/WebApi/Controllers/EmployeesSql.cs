
using HrSystem;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesSqlController : ControllerBase
{
    private readonly HrContext hrContext;
    private readonly ILogger<EmployeesController> logger;

    public EmployeesSqlController(ILogger<EmployeesController> logger, HrContext hrContext)
    {
        this.logger = logger;
        this.hrContext = hrContext;
    }

    [HttpGet]
    public IEnumerable<Employee> Get()
    {
        logger.LogInformation("Getting all employees");
        
        return hrContext.Employees.ToList();
    }

    public IActionResult Post()
    {
        return Forbid();
    }
}