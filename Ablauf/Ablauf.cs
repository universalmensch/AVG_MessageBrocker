using System;
using System.Text;
using RabbitMQ.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Ablauf;
using Daten;

namespace Ablauf{
    class Programm{
        static void Main(){

            //Zuerst Sender, dann Consumer gestartet.
            Sender sender = new Sender();
            Receiver receiver = new Receiver();

            //Zuerst Consumer, dann Sender gestartet.
            //Receiver receiver = new Receiver();
            //Sender sender = new Sender();

            //Sender erst nach abschicken der Anfrage gestartet.
            //Receiver receiver = new Receiver();

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

            //Sender erst nach abschicken der Anfrage gestartet.
            //Sender sender = new Sender();
            
            Console.ReadLine();  
        }

        /// <summary>
        /// Methode zum aufbauen einer Verbindung.
        /// </summary>
        /// <returns></returns>
        public static IModel getConnectionFactory(){
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            return connection.CreateModel();
        }

        /// <summary>
        /// Methode zum erstellen der Ergebnisqueue.
        /// </summary>
        /// <param name="channel"></param>
        public static void declareErgebnisQueue(IModel channel){
            channel.QueueDeclare(queue: "ergebnis",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        }

        /// <summary>
        /// Methode zum erstellen der Anfragequeue.
        /// </summary>
        /// <param name="channel"></param>
        public static void declareAnfrageQueue(IModel channel){
            channel.QueueDeclare(queue: "anfrage",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        }
    }
}
