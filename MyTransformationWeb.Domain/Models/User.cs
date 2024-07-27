using Newtonsoft.Json;

namespace MyTransformationWeb.Domain.Models;

public class User
{
    [JsonProperty(nameof(Id))]
    public string Id { get; set; }

    [JsonProperty(nameof(Email))]
    public string Email { get; set; }

    [JsonProperty(nameof(FirstName))]
    public string FirstName { get; set; }

    [JsonProperty(nameof(LastName))]
    public string LastName { get; set; }

    [JsonProperty(nameof(Birthdate))]
    public DateTime? Birthdate { get; set; }

    [JsonProperty(nameof(ProfilePicture))]
    public string ProfilePicture { get; set; }

    [JsonProperty(nameof(CreatedAt))]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonProperty(nameof(UpdatedAt))]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
