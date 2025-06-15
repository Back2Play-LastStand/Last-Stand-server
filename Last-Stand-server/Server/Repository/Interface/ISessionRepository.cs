using Server.Model.Account.Entity;

namespace Server.Repository.Interface;

public interface ISessionRepository
{
    Task CreateSessionAsync(AccountSession session);
    Task<AccountSession?> GetValidSessionAsync(string sessionId);
    Task DeleteSessionAsync(string sessionId);
}