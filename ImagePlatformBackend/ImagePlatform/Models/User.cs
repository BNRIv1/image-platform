using System.ComponentModel.DataAnnotations;

namespace ImagePlatform.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "USER";
        public bool isUserPremium { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public ICollection<Post> Posts { get; set; }
        public string AuthToken { get; set; } = string.Empty;
    }
}
