using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order_Service.Models;
using Order_Service.Services;

namespace Order_Service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("place")]
        public IActionResult PlaceOrder([FromBody] List<OrderRequestItemDto> orderItems)
        {
            try
            {
                int userId = GetUserIdFromClaims();
                var orderId = _orderService.PlaceOrderAsync(userId, orderItems);
                return Ok(orderId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            int userId = GetUserIdFromClaims();
            var orders = _orderService.GetOrdersByUserId(userId);
            return Ok(orders);
        }

        [HttpGet("{orderId}", Name = "GetOrderById")]
        public IActionResult GetOrderById(int orderId)
        {
            int userId = GetUserIdFromClaims();
            var orderDetails = _orderService.GetOrderById(userId, orderId);
            return Ok(orderDetails);
        }

        [HttpDelete("{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            try
            {
                int userId = GetUserIdFromClaims();
                _orderService.DeleteOrder(userId, orderId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private int GetUserIdFromClaims()
        {
            var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user claim.");
            }
            return userId;
        }
    }
}
