using Dapper;
using MySqlConnector;
using Server.Model.Data.Player.Entity;
using Server.Repository.Interface;

namespace Server.Repository;

public class DataRepository : IDataRepository
{
    private readonly string _connectionString;

    public DataRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    private MySqlConnection CreateConnection() => new(_connectionString);
    
    public async Task<PlayerData?> GetByPlayerIdAsync(string playerId)
    {
        const string sql = @"
            SELECT id, player_id AS PlayerId, player_name AS PlayerName, is_new_account AS IsNewAccount
            FROM last_stand_player_data
            WHERE player_id = @PlayerId;
        ";
        using var connection = CreateConnection();
        await connection.OpenAsync();
        
        return await connection.QueryFirstOrDefaultAsync<PlayerData>(sql,  new { PlayerId = playerId });
    }

    public async Task<bool> IsNameTakenAsync(string playerName)
    {
        const string sql = @"
            SELECT COUNT(1)
            FROM last_stand_player_data
            WHERE player_name = @PlayerName;
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        var count = await connection.ExecuteScalarAsync<int>(sql, new { PlayerName = playerName });
        return count > 0;
    }

    public async Task AddPlayerDataAsync(PlayerData data)
    {
        const string sql = @"
        INSERT INTO last_stand_player_data (player_id, player_name, is_new_account)
        VALUES (@PlayerId, @PlayerName, @IsNewAccount);
    ";
        
        using var connection = CreateConnection();
        await connection.OpenAsync();

        await connection.ExecuteAsync(sql, data);
    }

    public async Task UpdatePlayerNameAndIsNewAccountAsync(string playerId, string playerName, bool isNewAccount)
    {
        const string sql = @"
        UPDATE last_stand_player_data
        SET player_name = @PlayerName,
            is_new_account = @IsNewAccount
        WHERE player_id = @PlayerId;
        ";
        
        using var connection = CreateConnection();
        await connection.OpenAsync();
        
        await connection.ExecuteAsync(sql, new { PlayerId = playerId, PlayerName = playerName, IsNewAccount = isNewAccount });
    }
}