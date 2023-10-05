using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noodles.Managers;
using Noodles.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Noodles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager _manager;
        public UserController(NoodlesDBContext context)
        {
            _manager = new UserManager(context);
        }

        // GET: api/<UserController>
       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            IEnumerable<User> item = _manager.GetAll();
            if (item.Count() == 0)
            {
                return NoContent();
            }
            return Ok(item);
        }

        //[HttpPost("assign-roles")]
        //public async Task<IActionResult> AssignRolesToUsersAction()
        //{
        //    try
        //    {
        //        // Call the AssignRolesToUsers method from UserManager
        //        await _manager.AssignRolesToUsers();

        //        return Ok("Roles assigned successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions or errors
        //        return StatusCode(500, "An error occurred: " + ex.Message);
        //    }
        //}

        // GET api/<UserController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            User? result = _manager.GetById(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
        // POST api/<UserController>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public ActionResult<User> Post([FromBody] User value)
        {
            try
            {
                User result = _manager.Add(value);
                return Created("api/user/" + result.UserId, result);
            }
            catch (Exception ex)
            when (ex is ArgumentNullException || ex is ArgumentOutOfRangeException)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<UserController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public ActionResult<User> Put(int id, [FromBody] User value)
        {
            try
            {
                User? result = _manager.Update(id, value);
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

        // DELETE api/<UserController>/5
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public ActionResult<User> Delete(int id)
        {
            User? result = _manager.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        // POST api/<UserController>/Login
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginViewModel model)
        {
            // Replace this logic with your authentication logic based on user credentials
            var user = await _manager.FindByNameAsync(model.Username);
            // if (model.Username == "validUsername" && model.Password == "validPassword")
            if (user != null)
            {
                // Use UserManager's CheckPasswordAsync to verify the password
                var isPasswordValid = await _manager.CheckPasswordAsync(user, model.Password);

                if (isPasswordValid)
                {
                    
                // Create claims for the authenticated user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    // Add additional claims if needed
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Customize authentication properties if needed
                };

                // Sign in the user using cookie-based authentication
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Ok(new { message = "Login successful" });
                }
            }

            // Return Unauthorized if login fails
            return Unauthorized(new { message = "Login failed" });
        }

        // POST api/<UserController>/Logout
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user using cookie-based authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok(new { message = "Logout successful" });
        }
    
    }
}
