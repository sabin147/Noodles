using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noodles.Managers;
using Noodles.Models;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Noodles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderManager _manager;

        public OrderController(OrderManager context)
        {
            _manager = context;
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
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] List<OrderItemDTO> orderItems)
        {
            try
            {
                // Get the user ID based on the logged-in user (you may need to implement this)
                int userId = 1; // Get the user ID here;
                var order = await _manager.CreateOrder(userId, orderItems);
                return Ok("Order created successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }   
}
