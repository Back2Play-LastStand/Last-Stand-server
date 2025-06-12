namespace Server.Model.Account.Dto.Request;

public class PlayerLoginRequest
{
    public string PlayerId { get; set; } = null!;
    public string Password { get; set; } = null!;
}