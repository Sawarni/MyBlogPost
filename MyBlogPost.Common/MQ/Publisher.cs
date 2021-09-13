using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogPost.Common.MQ
{
    public class Publisher
    {
        private readonly string hostname;
        private readonly string userName;
        private readonly string password;

        public Publisher(string hostname, string userName, string password)
        {
            this.hostname = hostname;
            this.userName = userName;
            this.password = password;
        }
        public void PublishToMessageQueue(string exchangeName, string routingKey, string eventData)
        {
            // TOOO: Reuse and close connections and channel, etc, 
            var factory = new ConnectionFactory()
            {
                HostName = hostname,
                UserName = userName,
                Password = password,

            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(eventData);
            channel.BasicPublish(exchange: exchangeName,
                                             routingKey: routingKey,
                                             basicProperties: null,
                                             body: body);
        }


    }
}
