namespace Server.Model.Data.Player.Dto.Response;

public class PlayerDataResponse
{
    public string PlayerId { get; set; } = null!;
    public string PlayerName { get; set; } = null!;
    public string Message { get; set; } = string.Empty;
    public bool IsNew { get; set; } = false;
}
