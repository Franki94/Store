using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Reflection;
using System.Text;

namespace Store.Messaging
{
    public class SubscriberQueue
    {
        private readonly ConnectionFactory _factory;

        public SubscriberQueue()
        {
            _factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
        }

        public string Suscriber()
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("testingqueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    string messageOut = null;
                    consumer.Received += (sender, e) => {
                        var body = e.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        messageOut = message;
                    };                    
                    channel.BasicConsume("testingqueue", true, consumer);
                    return messageOut;
                }
            }
        }
    }
}
