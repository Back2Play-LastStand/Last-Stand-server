using System.Security.Claims;
using Server.Model.Token.Dto;

namespace Server.Service.Interface;

public interface IJwtService
{
    TokenResponse GenerateTokens(string playerId, Claim[]? additionalClaims = null);
    string GenerateAccessToken(string playerId,DateTime expiration, Claim[]? additionalClaims = null);
    TokenResponse RefreshAccessToken(string playerId, string oldRefreshToken);
    string GenerateRefreshToken();
    bool ValidateRefreshToken(string playerId, string refreshToken);
    public void DeleteRefreshToken(string playerId);
}