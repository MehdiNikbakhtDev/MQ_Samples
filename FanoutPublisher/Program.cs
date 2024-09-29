using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;



IConnection conn;
IModel channel;

ConnectionFactory factory = new ConnectionFactory();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

conn = factory.CreateConnection();
channel = conn.CreateModel();

channel.ExchangeDeclare(
    "ex.fanout.WithProg",
    "fanout",
    true,
    false,
    null);

channel.QueueDeclare(
    "my.queue1.WithProg",
    true,
    false,
    false,
    null);

channel.QueueDeclare(
    "my.queue2.WithProg",
    true,
    false,
    false,
    null);

channel.QueueBind("my.queue1.WithProg", "ex.fanout.WithProg", "");
channel.QueueBind("my.queue2.WithProg", "ex.fanout.WithProg", "");

channel.BasicPublish(
    "ex.fanout.WithProg",
    "",
    null,
    Encoding.UTF8.GetBytes("Message 1")
    );

channel.BasicPublish(
    "ex.fanout.WithProg",
    "",
    null,
    Encoding.UTF8.GetBytes("Message 2")
    );
/*
channel.QueueDelete("my.queue1.WithProg");
channel.QueueDelete("my.queue2.WithProg");
channel.ExchangeDelete("ex.fanout.WithProg");
*/
channel.Close();
conn.Close();

Console.WriteLine("Press a key to exit.");
Console.ReadKey();