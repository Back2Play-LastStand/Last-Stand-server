using Dapper;
using Server.Model.Account.Entity;
using Server.Repository.Interface;
using MySqlConnector;

namespace Server.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly string _connectionString;

    public AccountRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    private MySqlConnection CreateConnection() => new(_connectionString);
    
    public async Task<bool> CheckExistsAsync(string playerId,  string email)
    {
        const string sql = "SELECT COUNT(1) FROM last_stand_player_login_data WHERE player_id = @PlayerId AND email = @Email";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        var count = await connection.ExecuteScalarAsync<int>(sql, new { PlayerId = playerId, Email = email });
        
        return count > 0;
    }

    public async Task AddAccountAsynce(PlayerLoginData account)
    {
        const string sql = "INSERT INTO last_stand_player_login_data (player_id, password) VALUES (@PlayerId, @Password)";

        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        await connection.ExecuteAsync(sql, account);
    }

    public async Task<PlayerLoginData?> FindByPlayerIdAsync(string playerId)
    {
        const string sql = "SELECT player_id, password FROM last_stand_player_login_data WHERE player_id = @PlayerId";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        return await connection.QuerySingleOrDefaultAsync<PlayerLoginData>(sql, new { PlayerId = playerId });
    }

    public async Task<string?> FindPlayerIdByEmailAsync(string email)
    {
        const string sql = "SELECT player_id FROM last_stand_player_login_data WHERE email = @Email";

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        return await connection.QuerySingleOrDefaultAsync<string?>(sql, new { Email = email });
    }

    public async Task<PlayerLoginData?> FindByPlayerIdAndEmailAsync(string playerId, string email)
    {
        const string sql = "SELECT * FROM last_stand_player_login_data WHERE player_id = @PlayerId AND email = @Email";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        return await connection.QuerySingleOrDefaultAsync<PlayerLoginData>(sql, new { playerId, Email = email });
    }

    public async Task UpdatePasswordAsync(string playerId, string newPassword)
    {
        const string sql = "UPDATE last_stand_player_login_data SET password = @Password WHERE player_id = @PlayerId";
    
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await connection.ExecuteAsync(sql, new { Password = newPassword, PlayerId = playerId });
    }
}