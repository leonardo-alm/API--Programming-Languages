using System.Text.Json.Serialization;

namespace ApiConsumer
{
    public class Language
    {
        [JsonPropertyName("year")]
        public string Year { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("Chief developer, company")]
        public string Chiefdevelopercompany { get; set; }
        [JsonPropertyName("Predecessor(s)")]
        public string Predecessors { get; set; }
    }
}

