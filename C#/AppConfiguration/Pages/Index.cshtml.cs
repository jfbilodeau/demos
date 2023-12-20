using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace JfWebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;

    public Settings Settings { get; set; }

    public string Message => _configuration["Message"] ?? "No message has been set.";

    public IndexModel(
        ILogger<IndexModel> logger, 
        IConfiguration configuration, 
        IOptionsSnapshot<Settings> settings
    )
    {
        _logger = logger;
        _configuration = configuration;
        Settings = settings.Value;
    }

    public void OnGet()
    {

    }
}
