using System;
using System.Text.Json.Serialization;

namespace HrSystem;

public class HrRequest
{
    [JsonPropertyName("timestamp")] public DateTime Timestamp { get; set; } = DateTime.Now;

    [JsonPropertyName("name")] public string Name { get; set; } = "";

    [JsonPropertyName("message")] public string Message { get; set; } = "";

    public override string ToString()
    {
        return $"{nameof(Timestamp)}: {Timestamp}, {nameof(Name)}: {Name}, {nameof(Message)}: {Message}";
    }
}