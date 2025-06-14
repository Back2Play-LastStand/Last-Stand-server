using Server.Model.Account.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<string?> FindPlayerIdByEmailAsync(string email)
    {
        var playerId = await _accountRepository.FindPlayerIdByEmailAsync(email);
        return playerId;
    }

    public async Task<bool> ResetPasswordAsync(string playerId, string email, string newPassword)
    {
        var account = await _accountRepository.FindByPlayerIdAndEmailAsync(playerId, email);
        if (account == null)
            return false;
        
        var newHashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
        
        await _accountRepository.UpdatePasswordAsync(playerId, newHashedPassword);
        
        return true;
    }

    public async Task<PlayerLoginData?> GetPlayerLoginDataByPlayerIdAsync(string playerId)
    {
        return await _accountRepository.GetPlayerLoginDataByPlayerIdAsync(playerId);
    }

    public async Task UpdateIsNewAccountAsync(string playerId, bool isNewAccount)
    {
        await _accountRepository.UpdateIsNewAccountAsync(playerId, isNewAccount);
    }
}