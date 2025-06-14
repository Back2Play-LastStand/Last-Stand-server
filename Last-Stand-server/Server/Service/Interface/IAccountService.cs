using Server.Model.Account.Entity;

namespace Server.Service.Interface;

public interface IAccountService
{
    Task<string?> FindPlayerIdByEmailAsync(string email);
    Task<bool> ResetPasswordAsync(string playerId, string email, string newPassword);
    Task<PlayerLoginData?> GetPlayerLoginDataByPlayerIdAsync(string playerId);
    Task UpdateIsNewAccountAsync(string playerId, bool isNewAccount);
    Task<bool?> CheckIsNewAccountByPlayerIdAsync(string playerId);
}