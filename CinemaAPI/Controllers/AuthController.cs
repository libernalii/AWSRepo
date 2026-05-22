using CinemaCore.Interfaces;
using CinemaCore.Models;
using CinemaCore.Models.Requests;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public IActionResult Register(UserRequest user)
        {
            var result = _service.Register(user);
            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var token = _service.Login(request.Email, request.Password);
            return Ok(new { token });
        }
    }
}
