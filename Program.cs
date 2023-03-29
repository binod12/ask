using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

        var configuration = builder.Build();

        var token = configuration["Token"];


        if (args.Length > 0)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("authorization", $"Bearer {token}");

            var content = new StringContent("{\"model\": \"text-davinci-003\",\"prompt\": \"" + args[0] + "\",\"max_tokens\": 100,\"temperature\": 0}",
            Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/completions", content);

            string responseString = await response.Content.ReadAsStringAsync();

            try
            {
                var dyData = JsonConvert.DeserializeObject<dynamic>(responseString);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"{dyData!.choices[0].text}");
                Console.ResetColor();

            }
            catch (Exception ex)
            {
                Console.WriteLine("---> Could not Deserialize the response");
                throw;
            }

        }
        else
        {
            Console.WriteLine("---> You need to provide some input");
        }
    }
}