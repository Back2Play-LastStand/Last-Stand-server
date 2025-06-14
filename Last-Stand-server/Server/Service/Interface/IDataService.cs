using Server.Model.Data.Player.Entity;

namespace Server.Service.Interface;

public interface IDataService
{
    Task<bool> IsNameTakenAsync(string playerName);
    Task<PlayerData?> GetByPlayerIdAsync(string playerId);
    Task AddPlayerDataAsync(PlayerData data, bool isNewAccount);
    Task UpdatePlayerNameAndIsNewAccountAsync (string playerId, string playerName, bool isNewAccount);
}