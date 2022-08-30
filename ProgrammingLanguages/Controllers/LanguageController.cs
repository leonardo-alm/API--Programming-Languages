using Microsoft.AspNetCore.Mvc;
using ProgrammingLanguages.Dtos;
using ProgrammingLanguages.Models;
using System.Text.Json;
using System.IO;

namespace ProgrammingLanguages.Controllers
{
    [ApiController]
    [Route("languages")]
    public class LanguageController : ControllerBase
    {
        private readonly ILogger<LanguageController> _logger;
        private List<Language> languages;

        public LanguageController(ILogger<LanguageController> logger)
        {
            _logger = logger;

        }

        [HttpGet]
        public async Task<ActionResult<Language>> Read()
        {
            using var reader = new StreamReader("./languages.json");
            string json = await reader.ReadToEndAsync();
            languages = JsonSerializer.Deserialize<List<Language>>(json);
            return Ok(languages);
        }

        [HttpGet("year/{year}")]
        public async Task<ActionResult<Language>> ReadLanguageByYear(string year)
        {
            using var reader = new StreamReader("./languages.json");
            string json = await reader.ReadToEndAsync();
            languages = JsonSerializer.Deserialize<List<Language>>(json);

            var languagesByYear = languages
                 .Where(y => y.Year == year)
                 .ToList();

            if (!languagesByYear.Any()) return NotFound(new List<Language>());

            return Ok(languagesByYear);
        }

        [HttpGet("name")]
        public async Task<ActionResult<Language>> ReadLanguageByName(string? name)
        {
            using var reader = new StreamReader("./languages.json");
            string json = await reader.ReadToEndAsync();
            languages = JsonSerializer.Deserialize<List<Language>>(json);

            var language = languages
                .Where(n => n.Name.ToLower() == name.ToLower())
                .FirstOrDefault();

            if (language == null) return Ok("No correspondence");

            return Ok(language);
        }

        [HttpPost]
        public async Task<ActionResult<object>> CreateLanguage([FromBody] Language request)
        {
            var reader = new StreamReader("./languages.json");
            string json = await reader.ReadToEndAsync();
            reader.Dispose();
            languages = JsonSerializer.Deserialize<List<Language>>(json);

            var language = new Language
            {
                Year = request.Year,
                Name = request.Name,
                Chiefdevelopercompany = request.Chiefdevelopercompany,
                Predecessors = request.Predecessors
            };

            languages.Add(language);

            var content = JsonSerializer.Serialize(languages);
            System.IO.File.WriteAllText("./languages.json", content);

            return Ok(language);
        }

        [HttpPut]
        public async Task<ActionResult<object>> UpdateLanguage([FromQuery] string name, [FromBody] Language request)
        {
            var reader = new StreamReader("./languages.json");
            string json = await reader.ReadToEndAsync();
            reader.Dispose();
            languages = JsonSerializer.Deserialize<List<Language>>(json);


            var languageToUpdate = languages
            .Where(l => l.Name.ToLower() == name.ToLower())
            .FirstOrDefault();

            if (languageToUpdate == null) return Ok("No correspondence");

            languages.Remove(languageToUpdate);

            var updatedLanguage = new Language
            {
                Name = name,
                Year = request.Year,
                Chiefdevelopercompany = request.Chiefdevelopercompany,
                Predecessors = request.Predecessors
            };

            languages.Add(updatedLanguage);

            var content = JsonSerializer.Serialize(languages);
            System.IO.File.WriteAllText("./languages.json", content);

            return Ok(updatedLanguage);

        }

        [HttpDelete("{name}")]
        public async Task<ActionResult<Language>> DeleteLanguage(string name)
        {
            var reader = new StreamReader("./languages.json");
            string json = await reader.ReadToEndAsync();
            reader.Dispose();
            languages = JsonSerializer.Deserialize<List<Language>>(json);

            var languageToDelete = languages
                .Where(l => l.Name.ToLower() == name.ToLower())
                .FirstOrDefault();

            if (languageToDelete == null) return NotFound("No correspondence");

            languages.Remove(languageToDelete);

            var content = JsonSerializer.Serialize(languages);
            System.IO.File.WriteAllText("./languages.json", content);

            return Ok();
        }
    }
}