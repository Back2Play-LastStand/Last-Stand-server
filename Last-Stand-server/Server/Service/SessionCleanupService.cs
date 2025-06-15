using Server.Repository.Interface;

namespace Server.Service;

public class SessionCleanupService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SessionCleanupService> _logger;
    private Timer? _timer;

    public SessionCleanupService(IServiceScopeFactory scopeFactory, ILogger<SessionCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Session cleanup service starting.");

        _timer = new Timer(async _ => await DoWork(), null, TimeSpan.Zero, TimeSpan.FromHours(1));

        return Task.CompletedTask;
    }
    
    private async Task DoWork()
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var sessionRepository = scope.ServiceProvider.GetRequiredService<ISessionRepository>();
            
            _logger.LogInformation("Deleting expired sessions...");
            await sessionRepository.DeleteExpiredSessionsAsync();
            _logger.LogInformation("Expired sessions deleted.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting expired sessions.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Session cleanup service finishing.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}