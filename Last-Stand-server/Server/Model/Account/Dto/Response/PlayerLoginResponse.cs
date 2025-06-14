namespace Server.Model.Account.Dto.Response;

public class PlayerLoginResponse
{
    public string PlayerId { get; set; } = null!;
    public string Message { get; set; } = string.Empty;

    public bool IsNewAccount { get; set; } = true;

    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
