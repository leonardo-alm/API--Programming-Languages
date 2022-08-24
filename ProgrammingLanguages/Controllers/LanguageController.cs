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
        public ActionResult<IEnumerable<Language>> Get()
        {
            using var reader = new StreamReader("./languages.json");
            string json = reader.ReadToEnd();
            languages = JsonSerializer.Deserialize<List<Language>>(json);
            return languages;
        }

        [HttpGet("byyear")]
        public ActionResult<IEnumerable<Language>> GetLanguageByYear([FromQuery]string year, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            using var reader = new StreamReader("./languages.json");
            string json = reader.ReadToEnd();
            languages = JsonSerializer.Deserialize<List<Language>>(json);

            if (!string.IsNullOrWhiteSpace(year))
            {
                var languagesByYear = languages
                    .Where(y => y.Year == year)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                
                if (languagesByYear == null)
                {
                    return NotFound();
                }
                return languagesByYear;
            }
            
            else
            {
                return languages;
            }
        }
        [HttpPost]
        public object CreateLanguage([FromBody] NewLanguage request)
        {
            var reader = new StreamReader("./languages.json");
            string json = reader.ReadToEnd();
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
            return null;

        }
    }
}