namespace Server.Model.Account.Dto.Request;

public class ResetPasswordRequest
{
    public string PlayerId  { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}