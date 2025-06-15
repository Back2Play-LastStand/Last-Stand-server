using Server.Model.Account.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class SessionService :  ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IAccountRepository _accountRepository;

    public SessionService(ISessionRepository sessionRepository,  IAccountRepository accountRepository)
    {
        _sessionRepository = sessionRepository;
        _accountRepository = accountRepository;
    }
    
    public async Task<string> CreateSessionAsync(string playerId)
    {
        var account = await _accountRepository.FindByPlayerIdAsync(playerId)
                      ?? throw new ArgumentException("Invalid playerId");

        var existingSession = await _sessionRepository.HasValidSessionByAccountIdAsync(account.Id);
        if (existingSession)
            throw new InvalidOperationException("Session already exists for this account.");
        
        var session = new AccountSession
        {
            SessionId = Guid.NewGuid().ToString(),
            AccountId = account.Id,
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
}