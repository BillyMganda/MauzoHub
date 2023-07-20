namespace MauzoHub.Domain.Entities
{
    public class User : BaseEntity
    {        
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;        
        public string Role { get; set; } = string.Empty;
        public bool isActive { get; set; }
        public string PasswordResetToken { get; set; } = string.Empty;
        public DateTime ResetTokenExpiration { get; set; }

        public User()
        {

        }

        public User(string firstName, string lastName, string email, string passwordSalt, string passwordHash, string role, bool _isActive)
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.Now;
            LastModified = DateTime.Now;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
            Role = role;
            isActive = _isActive;
        }
    }
}
