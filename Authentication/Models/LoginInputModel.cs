namespace Authentication.Models
{
    public class LoginInputModel
    {
        public string Email { get; set; } = "test@test.com";
        public string Password { get; set; } = "pass123";
        public bool RememberMe { get; set; } = false;
    }
}
