using Server.Model.Account.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class SessionService :  ISessionService
{
    private readonly ISessionRepository _sessionRepository;

    public SessionService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }
    
    public async Task<string> CreateSessionAsync(int accountId)
    {
        var session = new AccountSession
        {
            SessionId = Guid.NewGuid().ToString(),
            AccountId = accountId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        };
    
        await _sessionRepository.CreateSessionAsync(session);
        return session.SessionId;
    }

    public async Task<string?> GetPlayerIdBySessionIdAsync(string sessionId)
    {
        var session = await _sessionRepository.GetValidSessionAsync(sessionId);
        return session?.AccountId.ToString();
    }

    public async Task DeleteSessionAsync(string sessionId)
    {
        await _sessionRepository.DeleteSessionAsync(sessionId);
    }
}