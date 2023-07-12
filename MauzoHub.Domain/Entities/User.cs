﻿namespace MauzoHub.Domain.Entities
{
    public class User : BaseEntity
    {        
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;        
        public UserRole Role { get; set; }

        private User()
        {

        }

        public User(string firstName, string lastName, string email, string passwordSalt, string passwordHash, UserRole role)
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
        }
    }
}