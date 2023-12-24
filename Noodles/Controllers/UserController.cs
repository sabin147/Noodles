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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Noodles.Controllers
{
    [Route("api/[controller]")] 

    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager _manager;
        private readonly IConfiguration _configuration;
        //public UserController(IConfiguration configuration, NoodlesDBContext context)
        //{
        //    _configuration = configuration;
        //    _manager = new UserManager(context);
        //}
        public UserController(UserManager userManager, IConfiguration configuration, NoodlesDBContext context)
        {
            _manager = userManager;
            _configuration = configuration;
        }



        [HttpGet("current-user")]
        [Authorize]
        public ActionResult<object> GetCurrentUser()
        {
            try
            {
                // Retrieve the user's claims from the current identity
                var userName = User?.Identity?.Name;
                var userIdClaim = User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var email = User.FindFirstValue(ClaimTypes.Email);
                var role = User.FindFirstValue(ClaimTypes.Role);

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    // Log or handle the case where user ID is not found or not valid
                    return BadRequest("User ID not found or not valid");
                }

                return Ok(new { UserId = userId, UserName = userName, Email = email, Role = role });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it based on your application's requirements
                return BadRequest("Error retrieving user information");
            }
        }


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {

            try
            {
                // Check if the username already exists
                if (_manager.GetAll().Any(u => u.Username == request.Username))
                {
                    return BadRequest("Username already exists.");
                }

                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // Create a new User object
                var newUser = new User
                {
                    Username = request.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Email = request.Email
                };

                // Add the user to the database
                var result = _manager.Add(newUser);

                return Ok(result);
            }
            catch (Exception ex)

            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPost("login")]

        public async Task<ActionResult<string>> Login(LoginViewModel request)
        {
            //var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == request.Username);

            try
            {
                // Fetch the user from the database
                var user = _manager.GetAll().SingleOrDefault(u => u.Email == request.Email);

                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return BadRequest("Wrong Password. ");
                }

                string token = CreateToken(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
   

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email), // Include the user's email here
                new Claim(ClaimTypes.Role, "Admin") // Include the user's role here
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
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
         
    }
}
