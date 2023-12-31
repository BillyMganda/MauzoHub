﻿using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Oauth.Commands
{
    public class ForgotPasswordCommand : IRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
