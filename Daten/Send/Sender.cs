using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Ablauf;

namespace Daten.Send{
    class Sender{
        private IModel channel;
        public IModel Channel {
            get { return channel; }
            private set => channel = value;
        }

        public void senderstarten(){
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
                
                Console.WriteLine($" [x] Received {anfrage}");

                anfrageBearbeiten(anfrage);
            };

            channel.BasicConsume(queue: "anfrage",
                                autoAck: true,
                                consumer: consumer);
        }

        public void sendmessage(string ergebnis){
            var body = Encoding.UTF8.GetBytes(ergebnis);

            Console.WriteLine($" [x] send {ergebnis}");

            channel.BasicPublish(exchange: string.Empty,
                     routingKey: "ergebnis",
                     basicProperties: null,
                     body: body);
        }

        public void anfrageBearbeiten(string anfrage){
            sendmessage("300");
        }
    }
}