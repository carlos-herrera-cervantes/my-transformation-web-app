using Newtonsoft.Json;

namespace MyTransformationWeb.Domain.Models;

public class FailedHttpResponse
{
    [JsonProperty("message")]
    public string Message { get; set; }
}
