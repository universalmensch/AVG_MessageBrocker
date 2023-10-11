using System.Text;
using RabbitMQ.Client;
namespace AVG_MESSAGEBROKER.Send{
    class Send{
        public Model channel;

        Receive(){
            channel = getConnectionFactory();
            declareQueue(chanal);
        }
    }
}
/*
var message = "Hello World!";
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: string.Empty,
                     routingKey: "hello",
                     basicProperties: null,
                     body: body);
Console.WriteLine($" [x] Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();


