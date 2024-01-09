using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noodles.Managers;
using Noodles.Models;
using System.Security.Claims;

namespace Noodles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly NoodlesDBContext _context;
        private readonly OrderItemManager _manager;
        public OrderItemController(NoodlesDBContext context)
        {
            _manager = new OrderItemManager(context);
        }

        // GET: api/<OrderItemController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet, Authorize]
        public ActionResult<IEnumerable<OrderItem>> Get()
        {
            IEnumerable<OrderItem> item = _manager.GetAll();
            if (item.Count() == 0)
            {
                return NoContent();
            }
            return Ok(item);
        }

        // GET api/<OrderItemController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}"), Authorize]
        public ActionResult<OrderItem> Get(int id)
        {
            OrderItem? result = _manager.GetById(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id}"), Authorize]
        public ActionResult<OrderItem> Delete(int id)
        {
            OrderItem? result = _manager.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("basketHistory")]
        [Authorize]
        public IActionResult GetBasketHistory()
        {
            try
            {
                // Retrieve the authenticated user's ID from claims or wherever you store it
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return BadRequest("User ID not found or not valid.");
                }

                var basketHistory = _manager.GetBasketHistoryForUser(userId);
                return Ok(basketHistory);
            }
            catch (Exception ex)
            {
                // Log the error or handle it accordingly
                return StatusCode(500, "Internal Server Error");
            }
        }
        
    }
}
