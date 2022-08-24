using System.Text.Json.Serialization;

namespace ProgrammingLanguages.Models
{
    public class Language
    {
        public string Year { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("Chief developer, company")]
        public string Chiefdevelopercompany { get; set; }

        [JsonPropertyName("Predecessor(s)")]
        public string Predecessors { get; set; }
    }
}

