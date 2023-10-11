
public static void Main(){
    

    
}

public static ConnectionFactory getConnectionFactory(){
    var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
}

public static void declareQueue(Model channel){
    channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
}


using System.Text;
using RabbitMQ.Client;


using System.Net.Http;
using System.Threading.Tasks;

    static async Task Main()
    {
        try
        {
            // Setze die API-Endpunkt-URL
            string apiUrl = "https://solar.googleapis.com/v1/dataLayers:get";

            // Setze die Koordinaten für die Mitte der Region
            double latitude = 37.7749; // Beispiel-Latitudenwert
            double longitude = -122.4194; // Beispiel-Longitudenwert

            // Setze den Radius in Metern
            double radiusMeters = 100;

            // Setze den gewünschten Datenansichtstyp
            string view = "FULL_LAYERS"; // Beispielwert, ändere dies entsprechend deinen Anforderungen

            // Setze die API-Key oder Authentifizierungsheader, falls erforderlich
            string apiKey = "DEIN_API_KEY"; // Beispielwert, ersetze dies durch deinen API-Schlüssel

            // Baue die URL mit den Parametern
            string requestUrl = $"{apiUrl}?location={latitude},{longitude}&radiusMeters={radiusMeters}&view={view}";

            // Erstelle einen HttpClient
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

                var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

//const  message = response;
Console.WriteLine(response);
var body = Encoding.UTF8.GetBytes("a");

channel.BasicPublish(exchange: string.Empty,
                     routingKey: "hello",
                     basicProperties: null,
                     body: body);
Console.WriteLine($" [x] Sent {response}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler: {ex.Message}");
        }
    }


