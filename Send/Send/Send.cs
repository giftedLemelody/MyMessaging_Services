//using System;
//using RabbitMQ.Client;
//using System.Text;
using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {

            ConnectionFactory factory = new ConnectionFactory();
            //Login to RabbitMQ we may use our readline if we want to to collect this values below
            string user = "guest";
            string pass = "guest";
            string vhost = "/";
            string hostName = "localhost";


            factory.UserName = user;
            factory.Password = pass;
            factory.VirtualHost = vhost;
            factory.HostName = hostName; 

            IConnection conn = factory.CreateConnection();

            IModel channel = conn.CreateModel();

            channel.Close();
            conn.Close();


            string my_message = "";
            Console.WriteLine("Please Enter Your connect Name: ");
            string my_name = Console.ReadLine();
            my_message = ("Hello! Daddy it is me your son " + my_name + ", \n This is a message from " + Environment.UserName.ToString() + " Mechine sent on " +
            DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " \n Sent Via RabbitMQ ");
            Console.WriteLine(my_message);
  
        //    //disk_free_limit.absolute = "100MB"; 

            //var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = my_message;
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
