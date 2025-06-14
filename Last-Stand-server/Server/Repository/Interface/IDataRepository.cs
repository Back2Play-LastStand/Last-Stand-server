using Server.Model.Data.Player.Entity;

namespace Server.Repository.Interface;

public interface IDataRepository
{
    Task<PlayerData?> GetByPlayerIdAsync(string playerId);
    Task<bool> IsNameTakenAsync(string playerName);
    Task AddPlayerDataAsync(PlayerData data, bool isNewAccount);
    Task UpdatePlayerNameAndIsNewAccountAsync(string playerId, string playerName, bool isNewAccount);
}