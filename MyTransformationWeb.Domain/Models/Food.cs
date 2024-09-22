using Newtonsoft.Json;

namespace MyTransformationWeb.Domain.Models;

public class Food
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("measurement_unit")]
    public string MeasurementUnit { get; set; }

    [JsonProperty("portion")]
    public int Portion { get; set; }

    [JsonProperty("calories")]
    public float Calories { get; set; }

    [JsonProperty("protein")]
    public float Protein { get; set; }

    [JsonProperty("carbs")]
    public float Carbs { get; set; }

    [JsonProperty("fats")]
    public float Fats { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

public class FoodCreation
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("measurement_unit")]
    public string MeasurementUnit { get; set; }

    [JsonProperty("portion")]
    public int Portion { get; set; }

    [JsonProperty("calories")]
    public float Calories { get; set; }

    [JsonProperty("protein")]
    public float Protein { get; set; }

    [JsonProperty("carbs")]
    public float Carbs { get; set; }

    [JsonProperty("fats")]
    public float Fats { get; set; }
}
