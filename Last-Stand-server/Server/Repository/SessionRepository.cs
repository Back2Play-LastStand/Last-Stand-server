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
            INSERT INTO account_session (session_id, account_id, created_at, expired_at)
            VALUES (@SessionId, @AccountId, @CreatedAt, @ExpiredAt);
            ";
        
        await using var connection  = CreateConnection();
        
        await connection.ExecuteAsync(sql, session);
    }

    public async Task<AccountSession?> GetValidSessionAsync(string sessionId)
    {
        const string sql = @"
            SELECT 
                session_id AS SessionId, 
                account_id AS AccountId, 
                created_at AS CreatedAt, 
                expired_at AS ExpiredAt 
            FROM account_session
            WHERE session_id = @SessionId AND expired_at > UTC_TIMESTAMP()";
        
        await using var connection  = CreateConnection();
        
        var session = await connection.QueryFirstOrDefaultAsync<AccountSession>(sql, new { SessionId = sessionId });
        
        if (session != null)
            Console.WriteLine($"Session found: SessionId={session.SessionId}, AccountId={session.AccountId}");
        else
            Console.WriteLine("Session not found or expired.");
        
        return session;
    }

    public async Task<bool> HasValidSessionByAccountIdAsync(int accountId)
    {
        const string sql = @"
            SELECT COUNT(*) FROM account_session
            WHERE account_id = @AccountId AND expired_at > UTC_TIMESTAMP()";
        
        await using var connection = CreateConnection();
        var count = await connection.ExecuteScalarAsync<int>(sql, new { AccountId = accountId });
        return count > 0;
    }

    public async Task DeleteExpiredSessionsAsync()
    {
        const string sql = @"
            DELETE FROM account_session
            WHERE expired_at <= UTC_TIMESTAMP()";
        
        await using var connection = CreateConnection();
        await connection.ExecuteAsync(sql);
    }

    public async Task DeleteSessionsAsync(string sessionId)
    {
        const string sql = @"
            DELETE FROM account_session
            WHERE session_id = @SessionId";
        
        await using var connection = CreateConnection();
        await connection.ExecuteAsync(sql, new{ SessionId = sessionId });
    }
    
    public async Task DeleteSessionsByAccountIdAsync(int accountId)
    {
        const string sql = @"
        DELETE FROM account_session
        WHERE account_id = @AccountId";
    
        await using var connection = CreateConnection();
        await connection.ExecuteAsync(sql, new { AccountId = accountId });
    }
}
