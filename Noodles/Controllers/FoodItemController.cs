using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noodles.Managers;
using Noodles.Models;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Noodles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly FoodItemManager _manager;
        public FoodItemController(NoodlesDBContext context)
        {
            _manager = new FoodItemManager(context);
        }

        // GET: api/<FoodItemController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]//, Authorize]
        public ActionResult<IEnumerable<FoodItem>> Get()
        {
            IEnumerable<FoodItem> item = _manager.GetAll();
            if (item.Count() == 0)
            {
                return NoContent();
            }
            return Ok(item);
        }

        // GET api/<FoodItemController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}"), Authorize(Roles ="Admin")]
        public ActionResult<FoodItem> Get(int id)
        {
            FoodItem? result = _manager.GetById(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
        // POST api/<FoodItemController>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost, Authorize(Roles = "Admin")]
        public ActionResult<FoodItem> Post([FromBody] FoodItem value)
        {
            try
            {
                FoodItem result = _manager.Add(value);
                return Created("api/footitem/" + result.FoodItemId, result);
            }
            catch (Exception ex)
            when (ex is ArgumentNullException || ex is ArgumentOutOfRangeException)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<FoodItemController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}"), Authorize(Roles ="Admin,Customer")]
        public ActionResult<FoodItem> Put(int id, [FromBody] FoodItem value)
        {
            try
            {
                FoodItem? result = _manager.Update(id, value);
                if (result == null)
                {
                    return NotFound();
                }
                else
                    return Ok(result);
            }
            catch (Exception ex)
                when (ex is ArgumentNullException || ex is ArgumentOutOfRangeException)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<FoodItemController>/5
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public ActionResult<FoodItem> Delete(int id)
        {
            FoodItem? result = _manager.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("orderHistory")]
        [Authorize]
        public IActionResult GetFoodHistoryForUser()
        {
            try
            {
                // Retrieve the authenticated user's ID from claims or wherever you store it
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return BadRequest("User ID not found or not valid.");
                }

                var orderHistory = _manager.GetFoodHistoryForUser(userId);
                return Ok(orderHistory);
            }
            catch (Exception ex)
            {
                // Log the error or handle it accordingly
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
