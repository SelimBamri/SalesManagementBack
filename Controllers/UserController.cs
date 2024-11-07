using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SalesManagementBack.Data;
using SalesManagementBack.DTO.Requests;
using SalesManagementBack.DTO.Responses;
using SalesManagementBack.Entities;
using SalesManagementBack.JwtFeatures;  
using System.Security.Claims;

namespace SalesManagementBack.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly JwtHandler _jwtHandler;
        public UserController(UserManager<User> userManager, JwtHandler jwtHandler, AppDbContext context)
        {
            this._context = context;
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
                UserName = input.Email,
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

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteMyAccount()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Name)!.Value);
            if (user == null)
            {
                return NotFound("User not found");
            }
            await _userManager.DeleteAsync(user);
            return Ok(new
            {
                Message = "ok"
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAuthenticatedUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Name)!.Value);

            if (user == null)
            {
                return NotFound("User not found");
            }
            var resp = new MyProfileResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePhoto = user.ProfilePhoto,
                Email = user.Email,
                Position = user.Position,
            };
            return Ok(resp);
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var resp = _context.Users.Select(user => new MyProfileResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePhoto = user.ProfilePhoto,
                Email = user.Email,
                Position = user.Position,
            }).ToList(); 
            return Ok(resp);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateMyProfile(ProfileUpdateRequest input)
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Name)!.Value);
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (input.FirstName != null)
            {
                user.FirstName = input.FirstName;
            }
            if (input.LastName != null)
            {
                user.LastName = input.LastName;
            }
            if (input.Position != null)
            {
                user.Position = input.Position;
            }
            if (input.Email != null)
            {
                user.Email = input.Email;
            }
            if (input.ProfilePhoto != null)
            {
                string base64Data = input.ProfilePhoto.Substring(input.ProfilePhoto.IndexOf(',') + 1);
                user.ProfilePhoto = Convert.FromBase64String(base64Data);
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponse { Errors = errors });
            }
            var token = _jwtHandler.CreateToken(user);
            return Ok(new { Token = token });
        }

        [HttpPut]
        [Route("password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(PasswordEditRequest input)
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Name)!.Value);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var passwordChangeResult = await _userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);
            if (!passwordChangeResult.Succeeded)
            {
                var errors = passwordChangeResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }
            var token = _jwtHandler.CreateToken(user);
            return Ok(new { Token = token });
        }
    }
}
