using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Store.Messaging;
using Store.Order.Models;
using System;
using System.Linq;
using System.Reflection;

namespace Store.Order.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly SubscriberQueue _subscriberQueue;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, SubscriberQueue subscriberQueue)
        {
            _logger = logger;
            _subscriberQueue = subscriberQueue;
        }

        //[HttpGet]
        //public IActionResult Get([FromServices] IBookMapper bookMapper)
        //{
        //    var a = bookMapper.MapBookFrom(new Person(), new School());
        //    var rng = new Random();
        //    var message = _subscriberQueue.Suscriber();
        //    if (message != null)
        //    {
        //        return Ok(message);
        //    }
        //    return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray());
        //}
    }
}
