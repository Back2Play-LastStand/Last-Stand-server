﻿using Server.Model.Account.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class AuthService : IAuthService
{
    private readonly IAccountRepository _accountRepository;

    public AuthService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<bool> RegisterAsync(string playerId, string password, string email)
    {
        bool exists = await _accountRepository.CheckExistsAsync(playerId, email);
        if (exists)
            return false;
        
        var hasedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        await _accountRepository.AddAccountAsync(new PlayerLoginData
        {
            PlayerId = playerId,
            Password = hasedPassword,
            Email = email
        });
        
        return true;
    }

    public async Task<(bool IsSuccess, bool IsNewAccount)> LoginAsync(string playerId, string password)
    {
        var account = await _accountRepository.FindByPlayerIdAsync(playerId);
        if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
            return (false, false);

        return (true, account.IsNewAccount);
    }
}