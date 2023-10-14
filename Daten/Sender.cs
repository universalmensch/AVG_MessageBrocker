using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Ablauf;
using System.Globalization;

namespace Daten{
    class Sender{
        private IModel channel;
        public IModel Channel {
            get { return channel; }
            private set => channel = value;
        }

        public Sender(){
            channel = Programm.getConnectionFactory();
            Programm.declareAnfrageQueue(channel);
            Programm.declareErgebnisQueue(channel);
            receiveAnfrage();
        }

        public void receiveAnfrage(){
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var anfrage = Encoding.UTF8.GetString(body);
                
                //Console.WriteLine($" [x] Received {anfrage}");
                Programm.logInDatei(anfrage, $@"Logs\{Programm.logfile}");

                anfrageBearbeiten(anfrage);
            };

            channel.BasicConsume(queue: "anfrage",
                                autoAck: true,
                                consumer: consumer);
        }

        public void sendmessage(string ergebnis){
            var body = Encoding.UTF8.GetBytes(ergebnis);

            Programm.logInDatei($" [Sender] send {ergebnis}", $@"Logs\{Programm.logfile}");

            channel.BasicPublish(exchange: string.Empty,
                     routingKey: "ergebnis",
                     basicProperties: null,
                     body: body);
        }

        public async void anfrageBearbeiten(string anfrage){
            List<string> anfragewerte = anfrage.Split(new string [] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(werte => werte.Trim()).ToList();

            List<KeyValuePair<string, string>> geoDaten = await Geocoding.getGeoDaten(anfragewerte[0], anfragewerte[1], anfragewerte[3] + " " + anfragewerte[2]);
            CultureInfo cultureinfo = new CultureInfo("en-US");
            double latitude = double.Parse(geoDaten[0].Value, cultureinfo);
            double longitude = double.Parse(geoDaten[1].Value, cultureinfo);
            Programm.logInDatei($"\nLatitude: {latitude}\nLongitude: {longitude}", $@"Logs\{Programm.logfile}");

            int neigung = 0;
            int azimut = 0;
            double kwp = Convert.ToDouble(anfragewerte[4]);

            string ergebnis = await APISolar.Solarcast(latitude, longitude, neigung, azimut, kwp);
            Programm.logInDatei(ergebnis, $@"Vorhersagen\{Programm.vorhersageFileName}");
            sendmessage(ergebnis);
        }
    }
}