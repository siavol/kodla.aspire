using System.Text.Json.Serialization;

namespace Kodla.Api.Models;

public class Meetup
{
    [JsonPropertyName("meetupId")] public int Id { get; set; } // TODO: change to string
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
