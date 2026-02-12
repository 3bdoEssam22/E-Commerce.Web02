using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTtansferObjects.OrderDTOs;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class OrdersController(IServiceManager _serviceManager) : ControllerBase
    {
        //Create Order
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _serviceManager.OrderService.CreateOrderAsync(orderDto, email!);
            return Ok(order);

        }

        //Get DeliveryMethods
        [AllowAnonymous]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<DeliveryMethodDto>> GetDeliveryMethods()
        {
            var deliveryMethods = await _serviceManager.OrderService.GetAllDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }

        //Get orders By Emai;
        [HttpGet]
        public async Task<ActionResult<OrderToReturnDto>> GetOrdersByEmail()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _serviceManager.OrderService.GetAllOrdersAsync(email!);
            return Ok(orders);
        }

        //Get Order By Id
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id)
        {
            var order = await _serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
        }


    }
}
