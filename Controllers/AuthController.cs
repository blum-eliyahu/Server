using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.DTOs;
using Server.Models;
using BCrypt.Net;

namespace Server.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase {
        private readonly AppDbContext _db;
        public AuthController(AppDbContext db) { _db = db; }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto) {
            if (_db.Users.Any(u => u.Username == dto.Username))
                return Conflict(new { message = "Username exists" });

            var user = new User {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Registered" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto) {
            var user = _db.Users.FirstOrDefault(u => u.Username == dto.Username);
            if (user == null) return Unauthorized(new { message = "Invalid credentials" });

            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!valid) return Unauthorized(new { message = "Invalid credentials" });

            // Optionally return a simple success message (no JWT required for minimal)
            return Ok(new { message = "Logged in" });
        }
    }
}
