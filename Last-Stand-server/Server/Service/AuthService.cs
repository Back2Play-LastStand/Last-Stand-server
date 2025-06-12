using Server.Model.Account.Entity;
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
    
    public async Task<bool> RegisterAsync(string playerId, string password)
    {
        bool exists = await _accountRepository.CheckExistsAsync(playerId);
        if (exists)
            return false;
        
        var hasedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        await _accountRepository.AddAccountAsynce(new PlayerLoginData
        {
            PlayerId = playerId,
            Password = hasedPassword
        });
        
        return true;
    }

    public async Task<string?> LoginAsync(string playerId, string password)
    {
        var account = await _accountRepository.FindByPlayerIdAsync(playerId);
        if (account == null)
            return null;
        
        if (!BCrypt.Net.BCrypt.Verify(password, account.Password))
            return null;
        
        // 후에 JWT 토큰 발행
        return Guid.NewGuid().ToString();
    }
}