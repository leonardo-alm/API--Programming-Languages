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

        [HttpGet("year")]
        public async Task<ActionResult<Language>> ReadLanguageByYear([FromQuery] string year)
        {
            using var reader = new StreamReader("./languages.json");
            string json = await reader.ReadToEndAsync();
            languages = JsonSerializer.Deserialize<List<Language>>(json);

            if (!string.IsNullOrWhiteSpace(year))
            {
                var languagesByYear = languages
                    .Where(y => y.Year == year)
                    .ToList();

                var numberOfLanguages = languagesByYear.Count();

                if (numberOfLanguages == 0)
                {
                    return NotFound(new
                    {
                        message = "No language found"
                    });
                }
                var returnMessage = $"{numberOfLanguages} languages were created in the year {year}:";

                if (numberOfLanguages == 1)
                {
                    returnMessage = $"{numberOfLanguages} language was created in the year {year}:";
                }
                return Ok(languagesByYear);
            }
            
            else
            {
                return Ok(languages);
            }
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Language>> ReadLanguageByName(string name)
        {
            using var reader = new StreamReader("./languages.json");
            string json = await reader.ReadToEndAsync();
            languages = JsonSerializer.Deserialize<List<Language>>(json);

            var language = languages
                .Where(n => n.Name == name)
                .FirstOrDefault();
                    

                if (language == null)
                {
                    return NotFound(new
                    {
                        message = "No language found"
                    });
                }
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
        public async Task<ActionResult<object>> UpdateLanguage([FromQuery]string name, [FromBody] Language request)
        {
            var reader = new StreamReader("./languages.json");
            string json = await reader.ReadToEndAsync();
            reader.Dispose();
            languages = JsonSerializer.Deserialize<List<Language>>(json);


            var languageToUpdate = languages
            .Where(l => l.Name == name)
            .First();

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
                .Where(l => l.Name == name)
                .First();

            languages.Remove(languageToDelete);

            var content = JsonSerializer.Serialize(languages);
            System.IO.File.WriteAllText("./languages.json", content);

            return Ok();
        }
    }
}