using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesManagementBack.DTO.Requests;
using SalesManagementBack.DTO.Responses;
using SalesManagementBack.Entities;

namespace SalesManagementBack.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }

        [HttpPost]
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
    }
}
