using System;
using System.Text;
using RabbitMQ.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Ablauf;
using Daten;

namespace Ablauf{
    public class Programm{
        static string logfileName = Path.GetRandomFileName();
        public static string logfile = logfileName.Replace(".", "") + ".txt";
        public static string vorhersageFileName = "Vorhersage" + DateTime.Now.Date.ToString("dd.MM.yyyy") + ".txt" ;

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
            string land = Console.ReadLine()??  throw new Exception();
            Console.WriteLine("Stadt: ");
            string stadt = Console.ReadLine()?? throw new Exception();
            Console.WriteLine("Straße: ");
            string straße = Console.ReadLine()?? throw new Exception();
            Console.WriteLine("Hausnummer: ");
            string hausnummer = Console.ReadLine()?? throw new Exception();
            Console.WriteLine("Solarleistung: ");
            string solarleistung = Console.ReadLine()?? throw new Exception();

            logInDatei(DateTime.Now + "\n" + land + "," + stadt + "," + straße + "," + hausnummer + "," + solarleistung, $@"Logs\{logfile}");

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

            Console.WriteLine();

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
            
            Console.WriteLine();
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
            
            Console.WriteLine();
        }

        /// <summary>
        /// Methode zum bestätigen der Eingegangen Anfrage.
        /// </summary>
        /// <param name="anfrage"></param>
        public static void anfrageEingangBestätigen(string anfrage){
            Console.WriteLine($" [Sender] Received {anfrage}");
        }

        /// <summary>
        /// Methode zum bestätigen der abgeschickten Anfrage.
        /// </summary>
        /// <param name="anfrage"></param>
        public static void anfrageAbsendenBestätigen(string anfrage){
            Console.WriteLine($" [Receiver] Sended {anfrage}");
        }

        public static void logInDatei(string inhalt, string pfadname) {
            try
            {
                // Überprüfen, ob die Datei vorhanden ist und falls nicht wird sie erstellt.
                if (!File.Exists(pfadname))
                {
                    File.Create(pfadname).Close();
                }

                string vorhandenerInhalt = File.ReadAllText(pfadname);
                string gesamterInhalt = vorhandenerInhalt + Environment.NewLine + inhalt;

                // Schreibe den Inhalt in die Datei 
                File.WriteAllText(pfadname, gesamterInhalt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Schreiben in die Log-Datei: {ex.Message}");
            }
        }
    }
}
