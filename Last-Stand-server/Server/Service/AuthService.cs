using Server.Model.Account.Entity;
using Server.Model.Token.Dto;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class AuthService : IAuthService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IAccountRepository accountRepository, IJwtService jwtService)
    {
        _accountRepository = accountRepository;
        _jwtService = jwtService;
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

    public async Task<(TokenResponse? Token, bool IsNewAccount)> LoginAsync(string playerId, string password)
    {
        var account = await _accountRepository.FindByPlayerIdAsync(playerId);
        if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
            return (null, false);
        
        var isNewAccount = account.IsNewAccount;

        if (isNewAccount)
            await _accountRepository.UpdateIsNewAccountAsync(playerId, false);
        
        var tokens =  _jwtService.GenerateTokens(playerId);
        return (tokens,  isNewAccount);
    }
}