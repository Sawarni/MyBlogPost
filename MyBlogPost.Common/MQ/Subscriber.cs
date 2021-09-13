using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace MyBlogPost.Common.MQ
{
    public class Subscriber
    {

        private readonly string hostname;
        private readonly string userName;
        private readonly string password;

        public Subscriber(string hostname, string userName, string password)
        {
            this.hostname = hostname;
            this.userName = userName;
            this.password = password;
        }

        public void ListenToQueue(Action<string,string> ActOnMessageReceived,string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostname,
                UserName = userName,
                Password = password,

            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
             {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var type = ea.RoutingKey;
                 ActOnMessageReceived(type, message);
                
            };
            channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
        }
    }
}
