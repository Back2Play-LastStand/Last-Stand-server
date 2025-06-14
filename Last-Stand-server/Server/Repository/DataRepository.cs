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
            SELECT  D.player_id AS PlayerId, D.player_name AS PlayerName, L.is_new_account AS IsNewAccount
            FROM last_stand_player_data D
            JOIN last_stand_player_login_data L ON D.player_id = L.player_id
            WHERE D.player_id = @playerId
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

    public async Task AddPlayerDataAsync(PlayerData data, bool isNewAccount)
    {
        const string sql = @"
        INSERT INTO last_stand_player_data (player_id, player_name)
        VALUES (@PlayerId, @PlayerName);
    ";
        
        using var connection = CreateConnection();
        await connection.OpenAsync();

        await connection.ExecuteAsync(sql, new
        {
            data.PlayerId,
            data.PlayerName,
            IsNewAccount = isNewAccount
        });
    }
}