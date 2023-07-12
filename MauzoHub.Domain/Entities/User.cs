﻿namespace MauzoHub.Domain.Entities
{
    public class User : BaseEntity
    {        
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;        
        public string Role { get; set; } = string.Empty;
    }
}
