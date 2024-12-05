using System.Net.Http; // För HTTP-anrop
using System.Text.Json; // För JSON-hantering
using System.Threading.Tasks; // För async-metoder

namespace DotNetProjectsFetcher
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Fetching .NET Foundation projects...");

            // URL till GitHub API
            string url = "https://api.github.com/orgs/dotnet/repos";

            using (HttpClient client = new HttpClient())
            {
                // GitHub kräver en User-Agent header
                client.DefaultRequestHeaders.Add("User-Agent", "DotNetProjectsFetcher");

                try
                {
                    // Skicka HTTP GET-anrop
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Kontrollera om svaret är OK (statuskod 200)
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        // Deserialisera JSON till en lista av GitHubRepo-objekt
                        var repos = JsonSerializer.Deserialize<List<GitHubRepo>>(jsonResponse);

                        // Iterera genom listan och skriv ut information
                        foreach (var repo in repos)
                        {
                            Console.WriteLine($"Name: {repo.Name}");
                            Console.WriteLine($"Description: {repo.Description}");
                            Console.WriteLine($"URL: {repo.HtmlUrl}");
                            Console.WriteLine($"Homepage: {repo.Homepage}");
                            Console.WriteLine($"Watchers: {repo.Watchers}");
                            Console.WriteLine($"Last Push: {repo.PushedAt}");
                            Console.WriteLine(new string('-', 40)); // Separator
                        }
                    }

                    else
                    {
                        Console.WriteLine($"HTTP Request failed. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
