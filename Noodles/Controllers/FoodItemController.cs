using Microsoft.AspNetCore.Mvc;
using Noodles.Managers;
using Noodles.Models;

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
        [HttpGet]
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
        [HttpGet("{id}")]
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
        [HttpPost]
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
        [HttpPut("{id}")]
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
        [HttpDelete("{id}")]
        public ActionResult<FoodItem> Delete(int id)
        {
            FoodItem? result = _manager.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
       
        
    }
}
