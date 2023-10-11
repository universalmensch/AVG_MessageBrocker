using System;
using System.Text;
using RabbitMQ.Client;

namespace AVG_MESSAGEBROKER.Send{
    class Send{
        private readonly Model channel;
        public Model Channel {
            get { return channel;}
        }

        public Send(){
            channel = getConnectionFactory();
            declareQueue(chanal);
        }

        public void sendmessage(){
            var message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                     routingKey: "hello",
                     basicProperties: null,
                     body: body);
        }
    }
}
/*




