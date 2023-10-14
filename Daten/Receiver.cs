using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Ablauf;

namespace Daten{
    class Receiver{
        private IModel channel;
        public IModel Channel {
            get { return channel;}
            private set => channel = value;
        }

        public Receiver(){
            channel = Programm.getConnectionFactory();
            Programm.declareAnfrageQueue(channel);
            Programm.declareErgebnisQueue(channel);
            receiveErgebnis();
        }

        public void sendanfrage(string land, string stadt, string straße, string hausnummer, string solarleistung){
            var anfrage = land + "," + stadt + "," + straße + "," + hausnummer + "," + solarleistung;
            var body = Encoding.UTF8.GetBytes(anfrage);

            Programm.logInDatei($" [Consumer] abgeschickt {anfrage}", $@"Logs\{Programm.logfile}");

            channel.BasicPublish(exchange: string.Empty,
                     routingKey: "anfrage",
                     basicProperties: null,
                     body: body);
        }

        public void receiveErgebnis(){
            var consumer = new EventingBasicConsumer(channel);
            
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var ergebnis = Encoding.UTF8.GetString(body);
                Programm.logInDatei($"Received {ergebnis}", $@"Logs\{Programm.logfile}");
            };

            channel.BasicConsume(queue: "ergebnis",
                                autoAck: true,
                                consumer: consumer);
        }
    }
}