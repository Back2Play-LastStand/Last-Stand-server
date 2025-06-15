using Server.Service.Interface;

namespace Server.Service;

public class SessionService :  ISessionService
{
    public async Task<long> CreateSessionAsync(string playerId)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> GetPlayerIdBySessionIdAsync(string sessionId)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteSessionAsync(string sessionId)
    {
        throw new NotImplementedException();
    }
}