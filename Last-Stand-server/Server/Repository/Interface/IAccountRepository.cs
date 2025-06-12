using Server.Model.Account.Entity;

namespace Server.Repository.Interface;

public interface IAccountRepository
{
    Task<bool> CheckExistsAsync(string playerId);
    Task AddAccountAsynce(PlayerLoginData account);
    Task<PlayerLoginData?> FindByPlayerIdAsync(string playerId);
    Task<PlayerLoginData?> FindByEmailAsync(string email);
    Task<PlayerLoginData?> FindByPlayerIdAndEmailAsync(string playerId, string email);
    Task UpdatePasswordAsync(PlayerLoginData account);
}