using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace ApiConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool endApp = false;

            while (!endApp)
            {
                Console.WriteLine("\nChoose one option:\n");
                Console.WriteLine($"1 - Consult all programming languages");
                Console.WriteLine($"2 - Consult programming languages by year of creation");
                Console.WriteLine($"3 - Consult programming language by name");
                Console.WriteLine($"4 - Update language");
                Console.WriteLine($"5 - Add new language");
                Console.WriteLine($"6 - Delete language");
                Console.WriteLine($"7 - Clear console");
                Console.WriteLine($"8 - Exit \n"); ;

                string option = Console.ReadLine();
                Console.WriteLine();

                switch (option)
                {
                    case "1":
                        string url = "https://localhost:44338/languages/";
                        GetAllLanguages(url).GetAwaiter().GetResult();
                        break;
                    case "2":
                        Console.WriteLine("Inform the desired year:\n");
                        string year = Console.ReadLine();
                        url = "https://localhost:44338/languages/year/" + year;
                        GetLanguagesByYear(url, year).GetAwaiter().GetResult();
                        break;
                    case "3":
                        Console.WriteLine("Inform the desired language:\n");
                        string name = Console.ReadLine();
                        url = "https://localhost:44338/languages/name?name=" + name;
                        GetLanguageByName(url).GetAwaiter().GetResult();
                        break;
                    case "4":
                        Console.WriteLine("Inform the language to be updated:\n");
                        name = Console.ReadLine();
                        Console.WriteLine("\nInform the year of creation:\n");
                        year = Console.ReadLine();
                        Console.WriteLine("\nInform the chief developer and company:\n");
                        string chiefdevelopercompany = Console.ReadLine();
                        Console.WriteLine("\nInform the predecessors:\n");
                        string predecessors = Console.ReadLine();
                        url = "https://localhost:44338/languages?name=" + name;
                        UpdateLanguage(url, name, year, chiefdevelopercompany, predecessors).GetAwaiter().GetResult();
                        break;
                    case "5":
                        Console.WriteLine("Inform the language to be added:\n");
                        name = Console.ReadLine();
                        Console.WriteLine("\nInform the year of creation:\n");
                        year = Console.ReadLine();
                        Console.WriteLine("\nInform the chief developer and company:\n");
                        chiefdevelopercompany = Console.ReadLine();
                        Console.WriteLine("\nInform the predecessors:\n");
                        predecessors = Console.ReadLine();
                        url = "https://localhost:44338/languages/";
                        AddNewLanguage(url, name, year, chiefdevelopercompany, predecessors).GetAwaiter().GetResult();
                        break;
                    case "6":
                        Console.WriteLine("Inform the language to be deleted:\n");
                        name = Console.ReadLine();
                        url = "https://localhost:44338/languages/" + name;
                        DeleteLanguage(url, name).GetAwaiter().GetResult();
                        break;
                    case "7":
                        Console.Clear();
                        break;
                    case "8":
                        Console.WriteLine("Exiting");
                        endApp = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        public static async Task AddNewLanguage(string url, string name, string year, string chiefdevelopercompany, string predecessors)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                var newLanguage = new Language
                {
                    Name = name,
                    Year = year,
                    Chiefdevelopercompany = chiefdevelopercompany,
                    Predecessors = predecessors
                };
                var json = JsonSerializer.Serialize(newLanguage);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                httpClient.PostAsync(url, data);

                Console.WriteLine($"\n{name} was successfully added to the languages file\n");
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public static async Task GetAllLanguages(string url)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var languages = JsonSerializer.Deserialize<List<Language>>(content);

                foreach (var language in languages)
                {
                    Console.WriteLine($"\n{language.Name} was created in {language.Year} by {language.Chiefdevelopercompany} and inspired in {language.Predecessors}\n");
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }
        public static async Task GetLanguagesByYear(string url, string year)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                if (content == null) return;
                var languages = JsonSerializer.Deserialize<List<Language>>(content);

                string message = $"\n{languages.Count()} languages were created in {year}: \n";

                if (languages.Count() == 1) message = $"\n1 language was created in {year}: \n";
                if (languages.Count() == 0) message = $"\nNo language was created in {year} \n";

                Console.WriteLine(message);

                foreach (var language in languages)
                {
                    Console.WriteLine($"- {language.Name}\n");
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public static async Task GetLanguageByName(string url)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (content == "No correspondence") Console.WriteLine("\nThere is no such language in the file");
                else
                {
                    var language = JsonSerializer.Deserialize<Language>(content);
                    Console.WriteLine($"\n{language.Name} was created in {language.Year} by {language.Chiefdevelopercompany} and inspired in {language.Predecessors}\n");
                }                            
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public static async Task UpdateLanguage(string url, string name, string year, string chiefdevelopercompany, string predecessors)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                var newLanguage = new Language
                {
                    Name = name,
                    Year = year,
                    Chiefdevelopercompany = chiefdevelopercompany,
                    Predecessors = predecessors
                };

                var json = JsonSerializer.Serialize(newLanguage);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync(url, data);
                var content = await response.Content.ReadAsStringAsync();

                if (content == "No correspondence") Console.WriteLine("\nThere is no such language in the file");
                else
                {
                    Console.WriteLine($"\n{name} was successfully updated to the languages file\n");
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }

        }

        public static async Task DeleteLanguage(string url, string name)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                var response = await httpClient.DeleteAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (content == "No correspondence") Console.WriteLine("\nThere is no such language in the file");
                else
                {
                    Console.WriteLine($"\n{name} was successfully deleted from the languages file\n");
                }              
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}




