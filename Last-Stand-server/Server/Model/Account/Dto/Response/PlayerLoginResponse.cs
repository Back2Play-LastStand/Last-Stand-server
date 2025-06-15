namespace Server.Model.Account.Dto.Response;

public class PlayerLoginResponse
{
    public string PlayerId { get; set; } = null!;
    public string Message { get; set; } = string.Empty;
    public bool IsNewAccount { get; set; } = true;
}
