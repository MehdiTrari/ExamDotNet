using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Models;
using ECommerceApi.Services;

namespace ECommerceApi.Controllers
{
    [Route("orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public ActionResult<OrderResponse> CreateOrder([FromBody] OrderRequest request)
        {
            var result = _orderService.ProcessOrder(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new ErrorResponse { Errors = result.Errors });
            }

            return Ok(result.Response);
        }
    }
}
