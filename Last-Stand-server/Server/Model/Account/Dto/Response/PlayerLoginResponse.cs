namespace Server.Model.Account.Dto.Response;

public class PlayerLoginResponse
{
    public string PlayerId { get; set; } = null!;
    public string Message { get; set; } = string.Empty;
    
    // 추후 JWT 토큰 추가
}