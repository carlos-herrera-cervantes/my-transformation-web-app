using Newtonsoft.Json;

namespace MyTransformationWeb.Domain.Models;

public class ConsumptionCreation
{
    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("food_id")]
    public string FoodId { get; set; }

    [JsonProperty("moment")]
    public DateTime Moment { get; set; }
}

public class Consumption
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("user_id")]
    public string UserId { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("food_id")]
    public string FoodId { get; set; }

    [JsonProperty("calories")]
    public float Calories { get; set; }

    [JsonProperty("protein")]
    public float Protein { get; set; }

    [JsonProperty("carbs")]
    public float Carbs { get; set; }

    [JsonProperty("fats")]
    public float Fats { get; set; }

    [JsonProperty("moment")]
    public DateTime Moment { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonProperty("partial_food")]
    public PartialFood PartialFood { get; set; }
}

public class PartialFood
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("measurement_unit")]
    public string MeasurementUnit { get; set; }
}
