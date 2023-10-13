using System;
using System.Text;
using RabbitMQ.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Ablauf;
using Daten.Receive;
using Daten.Send;

namespace Ablauf{
    class Programm{
        static async Task Main(){
            Sender sender = new Sender();
            Receiver receiver = new Receiver();

            sender.senderstarten();
            receiver.receiverstarten();

            receiver.sendanfrage("Deutschland", "Karlsruhe", "Lindenplatz", "10");

            List<KeyValuePair<string, string>> geoDaten = await Geocoding.getGeoDaten("Germany","Karlsruhe","40+Moltkestraße");
            string latitude = geoDaten[0].Value;
            string longitude = geoDaten[1].Value;
            Console.WriteLine($"Latitude: {latitude}\nLongitude: {longitude}");

             // Ersetzen durch die Apiwerte von Melvin
            double lat = 49.0093047; // Beispiel-Latitudenwert
            double longitute = 8.4332347; // Beispiel-Longitudenwert
            int dec = 0; // Beispiel-Neigung
            int az = 0; // Beispiel-Azimut
            double kwp = 1.67; // Beispiel-installierte Leistung in kWp

            APISolar.Solarcast(lat, longitute, dec, az, kwp);

            Console.ReadLine();  
        }

        public static IModel getConnectionFactory(){
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            return connection.CreateModel();
        }

        public static void declareErgebnisQueue(IModel channel){
            channel.QueueDeclare(queue: "ergebnis",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        }

        public static void declareAnfrageQueue(IModel channel){
            channel.QueueDeclare(queue: "anfrage",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        }

        /*public static async void HttpResponseMessage googleApiAufrufen(){
            string apiUrl = "https://solar.googleapis.com/v1/dataLayers:get";

            double latitude = 37.7749; // Beispiel-Latitudenwert
            double longitude = -122.4194; // Beispiel-Longitudenwert

            double radiusMeters = 100;

            string view = "FULL_LAYERS";
            string apiKey = "DEIN_API_KEY";

            string requestUrl = $"{apiUrl}?location={latitude},{longitude}&radiusMeters={radiusMeters}&view={view}";

            using (HttpClient client = new HttpClient())
                    {
                        // Füge ggf. Authentifizierungsheader hinzu
                        // client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

                        // Sende die GET-Anfrage
                        HttpResponseMessage response = await client.GetAsync(requestUrl);

                        // Überprüfe den Statuscode der Antwort
                        if (response.IsSuccessStatusCode)
                        {
                            // Verarbeite die Antwort hier
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Console.WriteLine(responseBody);
                        }
                        else
                        {
                            Console.WriteLine($"Fehler: {response.StatusCode} - {response.ReasonPhrase}");
                        }
                    }
         }*/
    }
}
