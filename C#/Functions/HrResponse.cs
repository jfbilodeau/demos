using System;
using System.Text.Json.Serialization;

namespace HrSystem;

public class HrResponse
{
    [JsonPropertyName("timestamp")] public DateTime Timestamp { get; set; } = DateTime.Now;

    [JsonPropertyName("request")] public HrRequest HrRequest { get; set; } = new();

    [JsonPropertyName("approved")] public bool Approved { get; set; } = false;

    public override string ToString()
    {
        return $"{nameof(Timestamp)}: {Timestamp}, {nameof(HrRequest)}: {HrRequest}, {nameof(Approved)}: {Approved}";
    }
}