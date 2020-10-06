using Microsoft.AspNetCore.Mvc;
using Store.Order.Models.Dto;
using System.Threading.Tasks;

namespace Store.Order.Controllers
{
    [Route("orders")]
    public class OrderController : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> InsertOrder([FromBody] OrderRequest order)
        {
            await Mediator.Send(order);
            return Ok();
        }
    }
}