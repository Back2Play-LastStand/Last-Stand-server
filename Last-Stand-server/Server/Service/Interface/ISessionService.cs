namespace Server.Service.Interface;

public interface ISessionService
{
    Task<string> CreateSessionAsync(int accountId);
    Task<string?> GetPlayerIdBySessionIdAsync(string sessionId);
    Task DeleteSessionAsync(string sessionId);
}