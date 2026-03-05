using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using var client = new HttpClient();
        var content = new StringContent("{\"login\": \"pedro.almeida\", \"senha\": \"abc123\"}", Encoding.UTF8, "application/json");
        try
        {
            var response = await client.PostAsync("http://localhost:5100/api/User/login", content);
            Console.WriteLine("Status: " + response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Result: " + result);
        }
        catch(Exception e)
        {
            Console.WriteLine("Err: " + e.Message);
        }
    }
}
