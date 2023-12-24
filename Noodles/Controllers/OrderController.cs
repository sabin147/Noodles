using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noodles.Managers;
using Noodles.Models;
using System.Data;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Noodles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderManager _manager;
        private readonly UserManager _userManager;
        private readonly SubscriptionManager _subscriptionManager;

        public OrderController(OrderManager context, UserManager userManager, SubscriptionManager subscriptionManager)
        {
            _manager = context;
            _userManager = userManager;
            _subscriptionManager = subscriptionManager;
        }


        //[Authorize(Roles = "Admin")]
        //[Route("api/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public ActionResult<IEnumerable<Order>> Get()
        {
            IEnumerable<Order> item = _manager.GetAll();
            if (item.Count() == 0)
            {
                return NoContent();
            }
            return Ok(item);
        }
        //[Authorize(Roles = "Admin")]
        //[Route("api/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Order> Get(int id)
        {
            Order? result = _manager.GetById(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }



        // Implement the endpoint to allow users to place an order
        //[Authorize(Roles = "Customer")]
        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> CreateOrder([FromBody] List<OrderItemDTO> orderItems)
        //{
        //    var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    Console.WriteLine($"User Id claim: {userIdClaim}");
        //    int userId;

        //    if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out userId))
        //    {
        //        // Log or handle the case where user ID is not found or not valid
        //        Console.WriteLine("User ID not found or not valid");
        //        return BadRequest("User ID not found or not valid");
        //    }

        //    Console.WriteLine($"Parsed User ID: {userId}");

        //    try
        //    {
        //        var order = await _manager.CreateOrder(userId, orderItems);
        //        Console.WriteLine($"Order created successfully for User ID: {userId}");
        //        return Ok("Order created successfully");
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        Console.WriteLine($"Error creating order: {ex.Message}");
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] List<OrderItemDTO> orderItems)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"User Id claim: {userIdClaim}");
            int userId;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out userId))
            {
                // Log or handle the case where user ID is not found or not valid
                Console.WriteLine("User ID not found or not valid");
                return BadRequest("User ID not found or not valid");
            }

            Console.WriteLine($"Parsed User ID: {userId}");

            try
            {
                var user =  _userManager.GetById(userId);

                if (user == null)
                {
                    Console.WriteLine($"User not found for ID: {userId}");
                    return BadRequest("User not found");
                }

                decimal subscriptionDiscountPercentage = 0;

                // Check if the user has a subscription
                if (user.SubscriptionId.HasValue)
                {
                    var subscription = _subscriptionManager.GetById(user.SubscriptionId.Value);
                    if (subscription != null)
                    {
                        subscriptionDiscountPercentage = subscription.DiscountPercentage;
                    }
                }

                var order = await _manager.CreateOrder(userId, orderItems, subscriptionDiscountPercentage);
                Console.WriteLine($"Order created successfully for User ID: {userId}");
                return Ok("Order created successfully");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

    }
}
