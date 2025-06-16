using Server.Service.Interface;
using StackExchange.Redis;

namespace Server.Service;

public class VerificationService : IVerificationService
{
    private readonly IDatabase _redis;
    private readonly TimeSpan _expiration =  TimeSpan.FromHours(3);

    public VerificationService(IConnectionMultiplexer  redis)
    {
        _redis = redis.GetDatabase();
    }
    
    public async Task StoreVerificationCodeAsync(string email, string code)
    {
        await _redis.StringSetAsync(email, code,  _expiration);
    }

    public async Task<bool> VerifyCodeAsync(string email, string code)
    {
        var storedCode = await _redis.StringGetAsync(email);
        if (storedCode.IsNullOrEmpty)
            return false;
        
        return storedCode == code;
    }

    public async Task RemoveCodeAsync(string email)
    {
        await _redis.KeyDeleteAsync(email);
    }
}