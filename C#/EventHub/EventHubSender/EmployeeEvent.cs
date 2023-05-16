using HrSystem;

namespace EventHubSender;

public class NewEmployeeEvent
{
    public DateTime time { get; } = DateTime.Now;
    public Employee? Employee { get; set; }
}