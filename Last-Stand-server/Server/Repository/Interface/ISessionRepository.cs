using Server.Model.Account.Entity;

namespace Server.Repository.Interface;

public interface ISessionRepository
{
    Task CreateSessionAsync(AccountSession session);
    Task<AccountSession?> GetValidSessionAsync(string sessionId);
    Task<bool> HasValidSessionByAccountIdAsync(int accountId);
    Task DeleteExpiredSessionsAsync();
    Task DeleteSessionsAsync(string sessionId);
    Task DeleteSessionsByAccountIdAsync(int accountId);
}