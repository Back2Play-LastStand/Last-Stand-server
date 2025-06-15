namespace Server.Model.Account.Dto.Response;

public class PlayerLoginResponse
{
    public string PlayerId { get; set; } = null!;
    public string? SessionId { get; set; }
    public bool IsNewAccount { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}
