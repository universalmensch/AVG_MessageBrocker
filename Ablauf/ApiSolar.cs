using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace Ablauf
{
    /// <summary>
    /// Klasse für den APISOLAR Aufruf.
    /// </summary>
    class APISolar{

        /// <summary>
        /// SolarcastAPI Aufruf.
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
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    string requestUrl = $"https://api.forecast.solar/estimate/{latitude}/{longitude}/{declination}/{azimuth}/{installedPower}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);

                        Programm.logInDatei($" [ApiSolar] Sent message to RabbitMQ", $@"Logs\{Programm.logfile}");

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