namespace Server.Model.Verify.Dto.Request;

public class EmailVerifyRequest
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
}