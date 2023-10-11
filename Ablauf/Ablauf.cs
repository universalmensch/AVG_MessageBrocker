using System.Text;
using RabbitMQ.Client;
using System.Net.Http;
using System.Threading.Tasks;
using AVG_MESSAGEBROKER.Receive;
using AVG_MESSAGEBROKER.Send;

namespace AVG_MESSAGEBROKER.Ablauf{
    class Programm{
        static void Main(){
            sender = new Send();
            receiver = new Receive();
        }

        public static Model getConnectionFactory(){
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            return connection.CreateModel();
        }

        public static void declareErgebnisQueue(Model channel){
            channel.QueueDeclare(queue: "ergebnis",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        }

        public static void declareAnfrageQueue(Model channel){
            channel.QueueDeclare(queue: "anfrage",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        }

        public static async HttpResponseMessage googleApiAufrufen(){
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
        }}

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
        catch (Exception ex){
            Console.WriteLine($"Fehler: {ex.Message}");
        }
    }
}
