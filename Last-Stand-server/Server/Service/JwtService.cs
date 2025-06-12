using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Configuration;
using Server.Model.Token.Dto;
using Server.Service.Interface;
using StackExchange.Redis;

namespace Server.Service;

public class JwtService : IJwtService
{
    private readonly JwtSetting _jwtSettings;
    private readonly IDatabase _redisDb;

    public JwtService(IOptions<JwtSetting> jwtOptions, IConnectionMultiplexer redis)
    {
        _jwtSettings = jwtOptions.Value;
        _redisDb = redis.GetDatabase();
    }
    
    public TokenResponse GenerateTokens(string playerId, Claim[]? additionalClaims = null)
    {
        var now = DateTime.UtcNow;
        var accessTokenExpiration = now.AddMinutes(_jwtSettings.ExpiryMinutes);
        var accessToken = GenerateAccessToken(playerId, accessTokenExpiration, additionalClaims);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = now.AddDays(7);

        var redisKey = $"token:refresh:{playerId}";
        _redisDb.StringSet(redisKey, refreshToken, refreshTokenExpiration - now);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiration = accessTokenExpiration,
            RefreshTokenExpiration = refreshTokenExpiration
        };
    }

    public string GenerateAccessToken(string playerId, DateTime expiration, Claim[]? additionalClaims = null)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, playerId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        if (additionalClaims != null)
            claims = claims.Concat(additionalClaims).ToArray();
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiration,
            signingCredentials: creds
        );
    
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public TokenResponse RefreshAccessToken(string playerId, string oldRefreshToken)
    {
        var redisKey = $"token:refresh:{playerId}";
        var storedToken = _redisDb.StringGet(redisKey);

        if (!storedToken.HasValue || storedToken != oldRefreshToken)
        {
            throw new SecurityTokenException("Invalid refresh token.");
        }

        _redisDb.KeyDelete(redisKey);

        var now = DateTime.UtcNow;
        var accessTokenExpiration = now.AddMinutes(_jwtSettings.ExpiryMinutes);
        var accessToken = GenerateAccessToken(playerId, accessTokenExpiration);

        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = now.AddDays(7);

        _redisDb.StringSet(redisKey, newRefreshToken, refreshTokenExpiration - now);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            AccessTokenExpiration = accessTokenExpiration,
            RefreshTokenExpiration = refreshTokenExpiration
        };
    }

    public string GenerateRefreshToken()
    {
        var random = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(random);
    }

    public bool ValidateRefreshToken(string playerId, string refreshToken)
    {
        var redisKey = $"token:refresh:{playerId}";
        var storedToken = _redisDb.StringGet(redisKey);
        return storedToken.HasValue && storedToken == refreshToken;
    }

    public void DeleteRefreshToken(string playerId)
    {
        var redisKey = $"token:refresh:{playerId}";
        _redisDb.KeyDelete(redisKey);
    }
}