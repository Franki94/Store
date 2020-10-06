using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace TestingQueue
{
    public class Message
    {
        public string Text { get; set; }
    }
    public class Person { }
    public class School { }

    public class Book { }

    public interface IBookMapper
    {
        Book MapBookFrom(Person person, School school);
    }

    public class BookMapper : IBookMapper
    {
        public Book MapBookFrom(Person person, School school)
        {
            return new Book();
        }
    }
    public class Program
    {

        public static async Task Main()
        {
            
            Console.ReadLine();
            //var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            //{
            //    sbc.Host("rabbitmq://localhost");

            //    sbc.ReceiveEndpoint("test_queue", ep =>
            //    {                    
            //        ep.Handler<Message>(context =>
            //        {
            //            return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
            //        });
            //    });
            //});

            //await bus.StartAsync(); // This is important!

            //await bus.Publish(new Message { Text = "Hi" });
            //await bus.Publish(new Message { Text = "Becker" });

            //Console.WriteLine("Press any key to exit");
            //await Task.Run(() => Console.ReadKey());

            //await bus.StopAsync();
        }
    }
}