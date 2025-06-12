namespace Server.Model.Account.Dto.Response;

public class ResetPasswordResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
}