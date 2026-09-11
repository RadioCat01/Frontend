using Microsoft.AspNetCore.Mvc;
using EnhanzerAPI.DTOs;
using EnhanzerAPI.Services;

namespace EnhanzerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Email and password are required" });
            }

            // Authenticate via external API
            var authResult = await _authService.LoginAsync(request.Email, request.Password);

            if (!authResult.Success)
            {
                return Unauthorized(new { message = authResult.Message });
            }

            // Generate JWT token
            var token = _tokenService.GenerateToken(request.Email);

            // Map locations to DTOs
            var locationDtos = authResult.Locations
                .Select(l => new LocationDetailDto
                {
                    LocationCode = l.LocationCode,
                    LocationName = l.LocationName
                })
                .ToList();

            // Return success response
            return Ok(new LoginResponseDto
            {
                Success = true,
                Token = token,
                Message = "Login successful",
                Locations = locationDtos
            });
        }
    }
}
