using RabbitMQ.Client;
using System.Text;
namespace DirectDemo;
class Program
{
    static void Main(string[] args)
    {
        IConnection conn;
        IModel channel;

        ConnectionFactory factory = new ConnectionFactory();
        // "guest"/"guest" by default, limited to localhost connections
        factory.HostName = "localhost";
        factory.VirtualHost = "/";
        factory.Port = 5672;
        factory.UserName = "guest";
        factory.Password = "guest";

        conn = factory.CreateConnection();
        channel = conn.CreateModel();

        channel.ExchangeDeclare
            (
            "ex.direct",
            "direct",
            true,
            false,
            null);

        channel.QueueDeclare(
            "DirectQueue.infos",
            true,
            false,
            false,
            null);

        channel.QueueDeclare(
            "DirectQueue.warnings",
            true,
            false,
            false,
            null);

        channel.QueueDeclare(
            "DirectQueue.errors",
            true,
            false,
            false,
            null);

        channel.QueueBind("DirectQueue.infos", "ex.direct", "info");
        channel.QueueBind("DirectQueue.warnings", "ex.direct", "warning");
        channel.QueueBind("DirectQueue.errors", "ex.direct", "error");

        channel.BasicPublish(
            "ex.direct",
            "infoss",
            null,
            Encoding.UTF8.GetBytes("Message with routing key info."));

        channel.BasicPublish(
            "ex.direct",
            "warning",
            null,
            Encoding.UTF8.GetBytes("Message with routing key warning."));

        channel.BasicPublish(
            "ex.direct",
            "error",
            null,
            Encoding.UTF8.GetBytes("Message with routing key error."));
    }
}
