namespace Server.Service.Interface;

public interface ISessionService
{
    Task<string> CreateSessionAsync(string playerId);
    Task<string?> GetPlayerIdBySessionIdAsync(string sessionId);
    Task<int?> GetAccountIdBySessionIdAsync(string sessionId);
}