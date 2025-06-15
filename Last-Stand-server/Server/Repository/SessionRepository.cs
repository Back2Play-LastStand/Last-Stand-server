using Dapper;
using MySqlConnector;
using Server.Model.Account.Entity;
using Server.Repository.Interface;

namespace Server.Repository;

public class SessionRepository : ISessionRepository
{
    private readonly string _connectionString;

    public SessionRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("AccountDbConnection");
    }
    
    private MySqlConnection CreateConnection() => new(_connectionString);
    
    public async Task CreateSessionAsync(AccountSession session)
    {
        const string sql = @"
            INSERT INTO account_session (session_id, account_id, created_at, expires_at)
            VALUES (@SessionId, @AccountId, @CreatedAt, @ExpiresAt);
            ";
        
        await using var connection  = CreateConnection();
        
        await connection.ExecuteAsync(sql, session);
    }

    public async Task<AccountSession?> GetValidSessionAsync(string sessionId)
    {
        const string sql = @"
            SELECT * FROM account_session
            WHERE session_id = @SessionId AND expires_at > UTC_TIMESTAMP()";
        
        await using var connection  = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<AccountSession>(sql, new { SessionId = sessionId });
    }

    public async Task<bool> HasValidSessionByAccountIdAsync(int accountId)
    {
        const string sql = @"
            SELECT COUNT(*) FROM account_session
            WHERE account_id = @AccountId AND expires_at > UTC_TIMESTAMP()";
        
        await using var connection = CreateConnection();
        var count = await connection.ExecuteScalarAsync<int>(sql, new { AccountId = accountId });
        return count > 0;
    }

    public async Task DeleteExpiredSessionsAsync()
    {
        const string sql = @"
            DELETE FROM account_session
            WHERE expires_at <= UTC_TIMESTAMP()";
        
        await using var connection = CreateConnection();
        await connection.ExecuteAsync(sql);
    }
}
