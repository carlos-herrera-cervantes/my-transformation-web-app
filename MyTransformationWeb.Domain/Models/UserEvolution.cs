using Newtonsoft.Json;

namespace MyTransformationWeb.Domain.Models;

public class UserEvolution
{
    [JsonProperty(nameof(Id))]
    public string Id { get; set; }

    [JsonProperty(nameof(UserId))]
    public string UserId { get; set; }

    [JsonProperty(nameof(ExerciseId))]
    public string ExerciseId { get; set; }

    [JsonProperty(nameof(Weight))]
    public int? Weight { get; set; }

    [JsonProperty(nameof(MeasurementUnit))]
    public string MeasurementUnit { get; set; }

    [JsonProperty(nameof(Reps))]
    public int? Reps { get; set; }

    [JsonProperty(nameof(Comment))]
    public string Comment { get; set; }

    [JsonProperty(nameof(Moment))]
    public DateTime? Moment { get; set; }

    [JsonProperty(nameof(CreatedAt))]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonProperty(nameof(UpdatedAt))]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
