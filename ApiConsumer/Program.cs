using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApiConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string url = "https://localhost:44338/languages";

            GetAllLanguages(url).GetAwaiter().GetResult();
        }
        public static async Task GetAllLanguages(string url)
        {
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            List<Language> languages = JsonSerializer.Deserialize<List<Language>>(content);

            foreach (var language in languages)
            {
                Console.WriteLine($"{language.Name} was created in {language.Year} by {language.Chiefdevelopercompany} and inspired by {language.Predecessors}");
            }
        }

    }

}



