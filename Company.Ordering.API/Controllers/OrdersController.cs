using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Company.Ordering.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> CreateOrderAsync()
        {
            return Created();
        }

        [HttpGet("{orderNumber}")]
        public async Task<IActionResult> GetOrderAsync(int orderNumber)
        {
            return Ok();
        }
    }
}
