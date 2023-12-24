using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Noodles.Managers;
using Noodles.Models;
using System.Security.Claims;

namespace Noodles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly SubscriptionManager _manager;
        private readonly UserManager _userManager;

        public SubscriptionController(SubscriptionManager subscriptionManager, UserManager userManager)
        {
            _manager = subscriptionManager;
            _userManager = userManager;
        }

        // GET: api/<SubscriptionController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]//, Authorize(Roles ="Admin")]
        public ActionResult<IEnumerable<Subscription>> Get()
        {
            IEnumerable<Subscription> item = _manager.GetAll();
            if (item.Count() == 0)
            {
                return NoContent();
            }
            return Ok(item);
        }

        // GET api/<SubscriptionController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Subscription> Get(int id)
        {
            Subscription? result = _manager.GetById(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
        // POST api/<SubscriptionController>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost] //, Authorize(Roles = "Admin")]
        public ActionResult<Subscription> Post([FromBody] Subscription value)
        {
            try
            {
                Subscription result = _manager.Add(value);
                return Created("api/footitem/" + result.SubscriptionId, result);
            }
            catch (Exception ex)
            when (ex is ArgumentNullException || ex is ArgumentOutOfRangeException)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<SubscriptionController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public ActionResult<Subscription> Put(int id, [FromBody] Subscription value)
        {
            try
            {
                Subscription? result = _manager.Update(id, value);
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

        // DELETE api/<SubscriptionController>/5
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public ActionResult<Subscription> Delete(int id)
        {
            Subscription? result = _manager.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        //[HttpPost("choose-subscription")]
        //[Authorize]
        //public ActionResult ChooseSubscription(int subscriptionId)
        //{
        //    try
        //    {
        //        // Get the user ID of the logged-in user
        //        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        //        {
        //            // Log or handle the case where user ID is not found or not valid
        //            return BadRequest("User ID not found or not valid");
        //        }

        //        var user = _userManager.GetById(userId);

        //        if (user == null)
        //        {
        //            return BadRequest("User not found.");
        //        }

        //        // Fetch the selected subscription from the database
        //        var subscription = _manager.GetById(subscriptionId);
        //        if (subscription == null)
        //        {
        //            return BadRequest("Invalid subscription selected.");
        //        }

        //        // Update the user's subscription
        //        user.SubscriptionId = subscriptionId;
        //        _userManager.Update(user.UserId, user);

        //        return Ok("Subscription selected successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpPost("choose-subscription")]
        [Authorize]
        public ActionResult ChooseSubscription(int? subscriptionId)
        {
            try
            {
                // Get the user ID of the logged-in user
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    // Log or handle the case where user ID is not found or not valid
                    return BadRequest("User ID not found or not valid");
                }

                var user = _userManager.GetById(userId);

                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                // If subscriptionId is null, it means the user is not selecting any subscription
                // which is removing the current subscription
                if (subscriptionId.HasValue)
                {
                    var subscription = _manager.GetById(subscriptionId.Value);

                    if (subscription == null)
                    {
                        return BadRequest("Invalid subscription selected.");
                    }

                    // Update the user's subscription
                    user.SubscriptionId = subscriptionId.Value;
                }
                else
                {
                    // If subscriptionId is null, remove the user's current subscription
                    user.SubscriptionId = null;
                }

                _userManager.Update(userId, user);

                return Ok("Subscription selected successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
