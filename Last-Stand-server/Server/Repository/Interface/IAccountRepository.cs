using Server.Model.Account.Entity;

namespace Server.Repository.Interface;

public interface IAccountRepository
{
    Task<bool> CheckExistsAsync(string playerId, string email);
    Task AddAccountAsync(PlayerLoginData account);
    Task<PlayerLoginData?> FindByPlayerIdAsync(string playerId);
    public Task UpdateIsNewAccountAsync(string playerId, bool isNewAccount);
    Task<string?> FindPlayerIdByEmailAsync(string email);
    Task<PlayerLoginData?> FindByPlayerIdAndEmailAsync(string playerId, string email);
    Task UpdatePasswordAsync(string playerId, string newPassword);
}