using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesManagementBack.DTO.Requests;
using SalesManagementBack.DTO.Responses;
using SalesManagementBack.Entities;
using SalesManagementBack.JwtFeatures;

namespace SalesManagementBack.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtHandler _jwtHandler;
        public UserController(UserManager<User> userManager, JwtHandler jwtHandler)
        {
            this._userManager = userManager;
            this._jwtHandler = jwtHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistration input)
        {
            if (input == null)
            {
                return BadRequest();
            }
            User user = new User {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Position = input.Position,
                Email = input.Email,
            };
            if (input.ProfilePhoto != null)
            {
                string base64Data = input.ProfilePhoto.Substring(input.ProfilePhoto.IndexOf(',') + 1);
                user.ProfilePhoto = Convert.FromBase64String(base64Data);
            }
            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded) { 
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponse{ Errors = errors})
                    ;
            }
            return StatusCode(201);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserForAuthentication input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email!);
            if (user == null || !await _userManager.CheckPasswordAsync(user, input.Password!))
            {
                return Unauthorized(new AuthenticationResponse { ErrorMessage = "Invalid authentication credentials." });
            }
            var token = _jwtHandler.CreateToken(user);
            return Ok(new AuthenticationResponse { IsAuthenticated = true, Token = token });
        }
    }
}
