using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Reflection;
using System.Text;

namespace Store.Messaging
{
    public class PublisherQueue
    {
        private readonly ConnectionFactory _factory;
        public PublisherQueue()
        {
            _factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
        }

        public void Publishing(string message) 
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("testingqueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var messageToQueue = new { Name = "Publisher", Message = message };                    
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageToQueue));

                    channel.BasicPublish("", "testingqueue", null, body);
                }
            }
        }
    }
}
