namespace Server.Service.Interface;

public interface IVerificationService
{
    Task StoreVerificationCodeAsync(string email, string code);
    Task<bool> VerifyCodeAsync(string email, string code);
    Task RemoveCodeAsync(string email);
}