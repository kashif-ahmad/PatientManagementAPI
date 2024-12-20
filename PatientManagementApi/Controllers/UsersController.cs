using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientManagementApi.DTOs;
using PatientManagementApi.Models;
using PatientManagementApi.UnitOfWork;
using PatientManagementApi.Utils;

namespace PatientManagementApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // Dependency Injection: Constructor Injection for Unit of Work
        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // POST: api/User/Register
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The user to register</param>
        /// <returns>Status 201 Created</returns>
        // Allow registration without a token
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = _unitOfWork.Users.GetUserByUsername(user.Username);
            if (existingUser != null)
            {
                return Conflict(new { Message = "Username already exists" });
            }

            user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
            _unitOfWork.Users.AddUser(user);
            _unitOfWork.Commit();
            return CreatedAtAction(null, new { id = user.UserId }, new { Message = "User registered successfully" });
        }


        // GET: api/User/{username}
        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>The user or NotFound</returns>
        [HttpGet("{username}")]
        public IActionResult GetUserByUsername(string username)
        {
            var user = _unitOfWork.Users.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Use the Unit of Work to access the Users repository
            var user = _unitOfWork.Users.GetUserByUsername(loginRequest.Username);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Verify the hashed password
            if (!PasswordHasher.VerifyPassword(loginRequest.PasswordHash, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password");
            }

            // Use JwtHelper to generate the token
            var token = JwtHelper.GenerateToken(user.Username, user.Role);

            return Ok(new
            {
                Message = "Login successful",
                Token = token,
                Role = user.Role
            });
        }
    }
}
