using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AVG_MESSAGEBROKER.Receiver{
    class Receiver{
        private readonly Model channel;
        public Model Channel {
            get { return channel;}
        }

        public Receiver(){
            channel = getConnectionFactory();
            declareAnfrageQueue(chanal);
            declareErgebnisQueue(chanal);
            receiveErgebnis();
        }

        public void sendanfrage(string land, string stadt, string straße, string hausnummer){
            var anfrage = land "," stadt "," straße "," hausnummer;
            var body = Encoding.UTF8.GetBytes(anfrage);

            Console.WriteLine($" [x] abgeschickt {anfrage}");

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
                Console.WriteLine($" [x] Received {ergebnis}");
            };

            channel.BasicConsume(queue: "ergebnis",
                                autoAck: true,
                                consumer: consumer);
        }
    }
}