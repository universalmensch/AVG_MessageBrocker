using System;
using System.Text;
using RabbitMQ.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Ablauf;
using Daten.Receive;
using Daten.Send;

namespace Ablauf{
    class Programm{
        static void Main(){
            Sender sender = new Sender();
            Receiver receiver = new Receiver();

            sender.senderstarten();
            receiver.receiverstarten();

            Console.WriteLine("Land: ");
            string land = Console.ReadLine();
            Console.WriteLine("Stadt: ");
            string stadt = Console.ReadLine();
            Console.WriteLine("Straße: ");
            string straße = Console.ReadLine();
            Console.WriteLine("Hausnummer: ");
            string hausnummer = Console.ReadLine();
            Console.WriteLine("Solarleistung: ");
            string solarleistung = Console.ReadLine();

            Console.WriteLine("\n" + land + "," + stadt + "," + straße + "," + hausnummer + "," + solarleistung);

            receiver.sendanfrage(land, stadt, straße, hausnummer, solarleistung);
            
            Console.ReadLine();  
        }

        public static IModel getConnectionFactory(){
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            return connection.CreateModel();
        }

        public static void declareErgebnisQueue(IModel channel){
            channel.QueueDeclare(queue: "ergebnis",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        }

        public static void declareAnfrageQueue(IModel channel){
            channel.QueueDeclare(queue: "anfrage",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        }
    }
}
