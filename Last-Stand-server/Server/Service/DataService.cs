using Server.Model.Data.Player.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class DataService : IDataService
{
    private readonly IDataRepository _dataRepository;

    public DataService(IDataRepository dataRepository)
    {
        _dataRepository = dataRepository;
    }
    
    public async Task<bool> IsNameTakenAsync(string playerName)
    {
        return await _dataRepository.IsNameTakenAsync(playerName);
    }

    public async Task<PlayerData?> GetByPlayerIdAsync(string playerId)
    {
        return await _dataRepository.GetByPlayerIdAsync(playerId);
    }

    public async Task AddPlayerDataAsync(PlayerData data, bool isNewAccount)
    {
        await _dataRepository.AddPlayerDataAsync(data, isNewAccount);
    }

    public async Task UpdatePlayerNameAndIsNewAccountAsync(string playerId, string playerName, bool isNewAccount)
    {
        await _dataRepository.UpdatePlayerNameAndIsNewAccountAsync(playerId, playerName, isNewAccount);
    }
}