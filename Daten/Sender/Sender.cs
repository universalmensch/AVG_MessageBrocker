using System;
using System.Text;
using RabbitMQ.Client;

namespace AVG_MESSAGEBROKER.Sender{
    class Sender{
        private readonly Model channel;
        public Model Channel {
            get { return channel;}
        }

        public Sender(){
            channel = getConnectionFactory();
            declareAnfrageQueue(channel);
            declareErgebnisQueue(channel);
            receiveAnfrage();
        }

        public void receiveAnfrage(){
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var anfrage = Encoding.UTF8.GetString(body);
                
                Console.WriteLine($" [x] Received {anfrage}");
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
    }
}
/*




