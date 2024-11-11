using Microsoft.AspNetCore.Identity;

namespace UserManagmentService.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } =string .Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}
