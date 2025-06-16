using Server.Service.Interface;
using StackExchange.Redis;

namespace Server.Service;

public class VerificationService : IVerificationService
{
    private readonly IDatabase _redis;
    private readonly TimeSpan _expiration = TimeSpan.FromMinutes(3);

    public VerificationService(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }

    private string GetRedisKey(string email) => $"verify:email:{email}";

    public async Task StoreVerificationCodeAsync(string email, string code)
    {
        var key = GetRedisKey(email);
        await _redis.StringSetAsync(key, code, _expiration);
    }

    public async Task<bool> VerifyCodeAsync(string email, string code)
    {
        var key = GetRedisKey(email);
        var storedCode = await _redis.StringGetAsync(key);

        if (storedCode.IsNullOrEmpty || storedCode != code)
            return false;

        await _redis.KeyDeleteAsync(key);
        
        var verifiedKey = GetRedisKey(email);
        await _redis.StringSetAsync(verifiedKey, "true", _expiration);
        
        return true;
    }

    public async Task RemoveCodeAsync(string email)
    {
        var key = GetRedisKey(email);
        await _redis.KeyDeleteAsync(key);
    }
    
    public async Task MarkEmailVerifiedAsync(string email)
    {
        var key = $"verified:email:{email}";
        await _redis.StringSetAsync(key, "true", TimeSpan.FromMinutes(5));
    }
}