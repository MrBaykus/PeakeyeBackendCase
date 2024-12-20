using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeakeyeBackendCase.Models;
using PeakeyeBackendCase.Services;

namespace PeakeyeBackendCase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var user = _authService.Authenticate(loginDto);
            if (user == null) return Unauthorized("Invalid credentials");

            var token = _authService.GenerateJwtToken(user);
            var otp = _authService.GenerateOtp(user);
            _authService.SendOtp(user, otp);

            return Ok(new { Token = token });
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDto otpDto)
        {
            var isValid = _authService.VerifyOtp(otpDto.Username, otpDto.Otp);
            if (!isValid) return Unauthorized("Invalid or expired OTP");

            var user = _authService.GetUserByUsername(otpDto.Username);
            var token = _authService.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are an Admin!");
        }

        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok("This is a public endpoint.");
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            var existingUser = _authService.GetUserByUsername(registerDto.Username);
            if (existingUser != null) return BadRequest("Username already exists");

            var user = _authService.Register(registerDto);
            var otp = _authService.GenerateOtp(user);
            _authService.SendOtp(user, otp);

            return Ok("Registration successful. Please verify the OTP sent to your email.");
        }

    }
}
