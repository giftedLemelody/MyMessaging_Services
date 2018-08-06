using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    class Program
    {
        //static void Main(string[] args)

        string message = "";
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "hello",
                                     autoAck: true,
                                     consumer: consumer);
                call_Respond();
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private void call_Respond()
        {
            ConnectionFactory factory = new ConnectionFactory(); 
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


            string my_message = "Hello " + message + " well received, I am your father!";
             
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
        }
    }
}
