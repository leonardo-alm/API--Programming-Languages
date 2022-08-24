using Microsoft.AspNetCore.Mvc;
using ProgrammingLanguages.Models;
using System.Text.Json;

namespace ProgrammingLanguages.Controllers
{
    [ApiController]
    [Route("language")]
    public class LanguageController : ControllerBase
    {
        private readonly ILogger<LanguageController> _logger;
        private List<Language> languages;

        public LanguageController(ILogger<LanguageController> logger)
        {
            _logger = logger;
            using var reader = new StreamReader("./languages.json");
            string json = reader.ReadToEnd();
            languages = JsonSerializer.Deserialize<List<Language>>(json);
        }

        [HttpGet(Name = "GetLanguages")]
        public IEnumerable<Language> Get()
        {
            return languages;
        }
    }
}