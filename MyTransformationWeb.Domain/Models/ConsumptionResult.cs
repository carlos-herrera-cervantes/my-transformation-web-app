using Newtonsoft.Json;

namespace MyTransformationWeb.Domain.Models;

public class ConsumptionResult
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("user_id")]
    public string UserId { get; set; }

    [JsonProperty("calories")]
    public float Calories { get; set; }

    [JsonProperty("protein")]
    public float Protein { get; set; }

    [JsonProperty("carbs")]
    public float Carbs { get; set; }

    [JsonProperty("fats")]
    public float Fats { get; set; }

    [JsonProperty("moment")]
    public string Moment { get; set; }

    [JsonProperty("comment")]
    public string Comment { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
