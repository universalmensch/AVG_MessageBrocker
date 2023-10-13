using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace Ablauf
{
    class APISolar{

        /// <summary>
        /// SolarcastAPI aufruf
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="declination"></param>
        /// <param name="azimuth"></param>
        /// <param name="installedPower"></param>
        public static async Task<string> Solarcast(double latitude, double longitude, int declination, int azimuth, double installedPower)
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
                            return formattedJson;

                    }
                    else
                    {
                    Console.WriteLine($"Fehler: {response.StatusCode} - {response.ReasonPhrase}");
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Fehlerdetails: {errorDetails}");                }
                }
                
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler: {ex.Message}");
                    throw new Exception();
                }
        }
    }
}