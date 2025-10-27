namespace Server.DTOs {
    public class RegisterDto {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class LoginDto {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
