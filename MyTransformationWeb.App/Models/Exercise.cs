using Newtonsoft.Json;

namespace MyTransformationWeb.App.Models;

public class Exercise
{
    [JsonProperty(nameof(Id))]
    public string Id { get; set; }

    [JsonProperty(nameof(Name))]
    public string Name { get; set; }

    [JsonProperty(nameof(Image))]
    public string Image { get; set; }

    [JsonProperty(nameof(MuscleGroups))]
    public string MuscleGroups { get; set; }

    [JsonProperty(nameof(CreatedAt))]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonProperty(nameof(UpdatedAt))]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
