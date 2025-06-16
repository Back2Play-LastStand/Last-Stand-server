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
        _connectionString = config.GetConnectionString("AccountDbConnection");
    }

    private MySqlConnection CreateConnection() => new(_connectionString);
    
    public async Task<bool> CheckExistsAsync(string playerId,  string email)
    {
        const string sql = "SELECT COUNT(1) FROM player_account_data WHERE player_id = @PlayerId AND email = @Email";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        var count = await connection.ExecuteScalarAsync<int>(sql, new { PlayerId = playerId, Email = email });
        
        return count > 0;
    }

    public async Task<bool> CheckIsNewAccountByPlayerIdAsync(string playerId)
    {
        const string sql = @"
            SELECT is_new_account
            FROM player_account_data
            WHERE player_id = @PlayerId
            LIMIT 1;";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        var isNewAccount = await connection.QueryFirstOrDefaultAsync<bool?>(sql, new { PlayerId = playerId });

        return isNewAccount ?? false;
    }

    public async Task AddAccountAsync(PlayerLoginData account)
    {
        const string sql = "INSERT INTO player_account_data (player_id, password, email) VALUES (@PlayerId, @Password, @Email)";

        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        await connection.ExecuteAsync(sql, account);
    }

    public async Task<PlayerLoginData?> FindByPlayerIdAsync(string playerId)
    {
        const string sql = @"SELECT  id, player_id AS PlayerId, password AS Password, email, is_new_account AS IsNewAccount
                            FROM player_account_data 
                            WHERE player_id = @PlayerId;";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        return await connection.QuerySingleOrDefaultAsync<PlayerLoginData>(sql, new { PlayerId = playerId });
    }

    public async Task UpdateIsNewAccountAsync(string playerId, bool isNewAccount)
    {
        const string sql = "UPDATE player_account_data SET is_new_account = @IsNewAccount WHERE player_id = @PlayerId";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        await connection.ExecuteAsync(sql, new { IsNewAccount = isNewAccount, PlayerId = playerId });
    }

    public async Task<string?> FindPlayerIdByEmailAsync(string email)
    {
        const string sql = "SELECT player_id FROM player_account_data WHERE email = @Email";

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        return await connection.QuerySingleOrDefaultAsync<string?>(sql, new { Email = email });
    }

    public async Task<PlayerLoginData?> FindByPlayerIdAndEmailAsync(string playerId, string email)
    {
        const string sql = "SELECT * FROM player_account_data WHERE player_id = @PlayerId AND email = @Email";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        return await connection.QuerySingleOrDefaultAsync<PlayerLoginData>(sql, new { playerId, Email = email });
    }

    public async Task UpdatePasswordAsync(string playerId, string newPassword)
    {
        const string sql = "UPDATE player_account_data SET password = @Password WHERE player_id = @PlayerId";
    
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await connection.ExecuteAsync(sql, new { Password = newPassword, PlayerId = playerId });
    }

    public async Task<PlayerLoginData?> GetPlayerLoginDataByPlayerIdAsync(string playerId)
    {
        const string query = @"
        SELECT id, player_id, password, email, is_new_account
        FROM player_account_data
        WHERE player_id = @PlayerId
        LIMIT 1";

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        return await connection.QueryFirstOrDefaultAsync<PlayerLoginData>(query, new { PlayerId = playerId });
    }

    public async Task<PlayerLoginData?> GetPlayerLoginDataByIdAsync(int id)
    {
        const string sql = @"
            SELECT id, player_id AS PlayerId, password AS Password, email, is_new_account AS IsNewAccount
            FROM player_account_data
            WHERE id = @Id
            LIMIT 1;";
        
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        
        return await connection.QueryFirstOrDefaultAsync<PlayerLoginData>(sql, new { Id = id });    
    }

    public async Task<bool> CheckPlayerIdExistsAsync(string playerId)
    {
        const string sql = "SELECT COUNT(1) FROM player_account_data WHERE player_id = @PlayerId";

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var count = await connection.ExecuteScalarAsync<int>(sql, new { PlayerId = playerId });

        return count > 0;
    }
}