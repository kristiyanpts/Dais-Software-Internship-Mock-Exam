namespace FinalProject.Services.DTOs.Requests.Authentication.Register
{
    public class RegisterRequestDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
    }
}