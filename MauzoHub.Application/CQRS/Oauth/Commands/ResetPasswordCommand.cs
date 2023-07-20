using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Oauth.Commands
{
    public class ResetPasswordCommand : IRequest
    {
        [Required, MinLength(6)]
        public string Token { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
        [Required, MinLength(6), Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
