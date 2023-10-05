using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noodles.Managers;
using Noodles.Models;

namespace Noodles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly OrderItemManager _manager;
        public OrderItemController(NoodlesDBContext context)
        {
            _manager = new OrderItemManager(context);
        }

        // GET: api/<OrderItemController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
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
        [HttpGet("{id}")]
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
        [HttpDelete("{id}")]
        public ActionResult<OrderItem> Delete(int id)
        {
            OrderItem? result = _manager.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
