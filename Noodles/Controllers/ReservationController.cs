using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noodles.Managers;
using Noodles.Models;
using System.Security.Claims;

namespace Noodles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationManager _manager;
        public ReservationController(ReservationManager manager)
        {
            _manager = manager;
        }

        // GET: api/<ReservationController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]//, Authorize(Roles ="Admin")]
        public ActionResult<IEnumerable<Reservation>> Get()
        {



            IEnumerable<Reservation> item = _manager.GetAll();
            if (item.Count() == 0)
            {
                return NoContent();
            }
            return Ok(item);
        }

        // GET api/<ReservationController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Reservation> Get(int id)
        {
            Reservation? result = _manager.GetById(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
        // POST api/<ReservationController>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        [Authorize]
        public ActionResult<Reservation> Post([FromBody] Reservation value)
        {
            try
            {
                // Get the logged-in user's ID from your authentication mechanism
                
                int loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                Reservation result = _manager.Add(value, loggedInUserId);
                return Created($"api/reservation/{result.ReservationId}", result);
            }
            catch (Exception ex)
            when (ex is ArgumentNullException || ex is ArgumentOutOfRangeException)

            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/<ReservationController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public ActionResult<Reservation> Put(int id, [FromBody] Reservation value)
        {
            try
            {
                Reservation? result = _manager.Update(id, value);
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

        // DELETE api/<ReservationController>/5
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public ActionResult<Reservation> Delete(int id)
        {
            Reservation? result = _manager.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
