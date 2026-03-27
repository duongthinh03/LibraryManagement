using Library.Application.Dtos.Login.Request;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            await _authService.RegisterAsync(request);
            return Ok("Register success");
        }

        [HttpGet("verify")]
        [AllowAnonymous]
        public async Task<IActionResult> Verify([FromQuery] string token)
        {
            try
            {
                await _authService.VerifyAccountAsync(token);

                return Ok(new
                {
                    message = "Verify thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize]
        [HttpGet("debug")]
        public IActionResult Debug()
        {
            return Ok(User.Claims.Select(x => new { x.Type, x.Value }));
        }
    }
}
