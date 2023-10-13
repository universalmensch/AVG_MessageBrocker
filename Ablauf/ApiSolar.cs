using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace Ablauf
{
    class APISolar{
        public static async void Solarcast(double latitude, double longitude, int declination, int azimuth, double installedPower)
        {
            try
            {
                // Erstelle den HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Setze den Accept-Header für JSON
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    // Baue die URL mit den Parametern
                    string requestUrl = $"https://api.forecast.solar/estimate/{latitude}/{longitude}/{declination}/{azimuth}/{installedPower}";

                    // Sende die GET-Anfrage
                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    // Überprüfe den Statuscode der Antwort
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("ffffffffffffffffffffffffffffffffffffffff");
                        // Verarbeite die Antwort hier
                        //string responseBody = await response.Content.ReadAsStringAsync();
                        //Console.WriteLine(responseBody);

                        // Verarbeite die Antwort hier
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);
                        Console.WriteLine(formattedJson);


                        // Sende die Antwort an RabbitMQ
                        var factory = new ConnectionFactory { HostName = "localhost" };
                        using var connection = factory.CreateConnection();
                        using var channel = connection.CreateModel();

                        channel.QueueDeclare(queue: "hello",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        var body = Encoding.UTF8.GetBytes(formattedJson);
                        channel.BasicPublish(exchange: string.Empty,
                            routingKey: "hello",
                            basicProperties: null,
                            body: body);
                            Console.WriteLine($" [x] Sent message to RabbitMQ");

                    }
                    else
                    {
                    Console.WriteLine($"Fehler: {response.StatusCode} - {response.ReasonPhrase}");
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Fehlerdetails: {errorDetails}");                }
                }
                
                }
                catch (Exception ex)
                {Console.WriteLine($"Fehler: {ex.Message}");}
                {
                
            }
        }
    }
}