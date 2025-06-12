namespace Server.Service.Interface;

public interface IAccountService
{
    Task<string?> FindPlayerIdByEmailAsync(string email);
    Task<bool> ResetPasswordAsync(string playerId, string email, string newPassword);
}