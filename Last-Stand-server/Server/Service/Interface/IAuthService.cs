using Server.Model.Account.Entity;

namespace Server.Service.Interface;

public interface IAuthService
{
    Task<bool> RegisterAsync(string playerId, string password);
    Task<string?> LoginAsync(string playerId, string password);
}