using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AVG_MESSAGEBROKER.Receive{
    class Receive{
        public Model channel;

        Receive(){
            channel = getConnectionFactory();
            declareQueue(chanal);
        }
    }
}
/*


Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
};
channel.BasicConsume(queue: "hello",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();