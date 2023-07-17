using MauzoHub.Domain.DTOs;
using System.Security.Claims;

namespace MauzoHub.Domain.Interfaces
{
    public interface IJWTManagerRepository
    {
        Tokens GenerateToken(string userName);
        Tokens GenerateRefreshToken(string userName);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
