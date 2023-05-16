using System.ComponentModel.DataAnnotations.Schema;

namespace HrSystem;

using System.Text.Json.Serialization;

public class Employee
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = "";

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = "";

    [JsonPropertyName("salary")]
    public double Salary { get; set; } = 0.0;

    [JsonPropertyName("dept")]
    [Column("dept")]
    public string Department { get; set; } = "";

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}, {nameof(Salary)}: {Salary}, {nameof(Department)}: {Department}";
    }
}
