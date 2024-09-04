namespace EnviBad.API.Common.Models
{
    public class UserInfo : IdInfo
    {
        public string? Login { get; set; }
        public string? PasswordHash { get; set; }
        public string? RefreshToken { get; set; }
        public bool? IsEnabled { get; set; } = true;
        public bool? IsAdmin { get; set; } = false;
    }
}
