using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noodles.Models;

namespace Noodles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestOrderController : ControllerBase
    {
        private readonly NoodlesDBContext _context;
        public TestOrderController(NoodlesDBContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] List<OrderItemDTO> orderItems)
        {
            // Validate the input and calculate the total amount
            decimal totalAmount = 0;
            var order = new Order
            {
                UserId = 1,// Set the user ID based on the logged-in user,
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in orderItems)
            {
                var foodItem = await _context.FoodItems.FindAsync(item.FoodItemId);
                if (foodItem == null)
                {
                    return BadRequest("Invalid food item ID: " + item.FoodItemId);
                }

                var orderItem = new OrderItem
                {
                    FoodItemId = item.FoodItemId,
                    Quantity = item.Quantity,
                    Subtotal = foodItem.Price * item.Quantity
                };

                totalAmount += orderItem.Subtotal;
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = totalAmount;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok("Order created successfully");
        }

    }
}
