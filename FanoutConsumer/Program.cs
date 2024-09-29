using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FanoutConsumer;
class Program
{
    static IConnection conn;
    static IModel channel;

    static void Main(string[] args)
    {
        ConnectionFactory factory = new ConnectionFactory();
        // "guest"/"guest" by default, limited to localhost connections
        factory.HostName = "localhost";
        factory.VirtualHost = "/";
        factory.Port = 5672;
        factory.UserName = "guest";
        factory.Password = "guest";

        conn = factory.CreateConnection();
        channel = conn.CreateModel();

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += Consumer_Received;

        var consumerTag = channel.BasicConsume("FanoutQueue.queue1.WithProg", false, consumer);
        //var consumerTag1 = channel.BasicConsume("my.queue2.WithProg", false, consumer);

        Console.WriteLine("Waiting for messages. Press any key to exit.");
        Console.ReadKey();
        conn.Close();
        channel.Close();
    }

    private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
    {
        string message = Encoding.UTF8.GetString(e.Body.ToArray());
        Console.WriteLine("Message:" + message);

        channel.BasicNack(e.DeliveryTag, false, false);
        //channel.BasicNack(e.DeliveryTag, false, true);
        //channel.BasicAck(e.DeliveryTag, false );
    }
  
}
