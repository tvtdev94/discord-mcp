using Discord;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordMcp.Infrastructure;

/// <summary>
/// Hosted service that manages the Discord client lifecycle:
/// connects on startup and disconnects on shutdown.
/// Blocks startup until the client fires Ready so all handlers
/// can safely access guilds from the moment the host is running.
/// </summary>
public sealed class DiscordClientHostedService : IHostedService
{
    private readonly DiscordSocketClient _client;
    private readonly OperatorContext _operatorContext;
    private readonly ILogger<DiscordClientHostedService> _logger;
    private readonly string _token;
    private readonly TaskCompletionSource _readySource = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public DiscordClientHostedService(
        DiscordSocketClient client,
        OperatorContext operatorContext,
        ILogger<DiscordClientHostedService> logger)
    {
        _client = client;
        _operatorContext = operatorContext;
        _logger = logger;
        _token = Environment.GetEnvironmentVariable("DISCORD_TOKEN")
                 ?? throw new InvalidOperationException("DISCORD_TOKEN environment variable is not set.");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _client.Ready += OnReady;
        _client.Log += OnLog;

        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();

        // Wait until the Discord gateway fires the Ready event before the host continues
        await _readySource.Task.WaitAsync(cancellationToken);
        _logger.LogInformation("Discord client is ready.");

        // Resolve operator identity now that the Discord client is fully connected
        await _operatorContext.InitializeAsync(_client, _logger);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _client.Ready -= OnReady;
        _client.Log -= OnLog;
        await _client.StopAsync();
    }

    private Task OnReady()
    {
        _readySource.TrySetResult();
        return Task.CompletedTask;
    }

    private Task OnLog(LogMessage msg)
    {
        var level = msg.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error    => LogLevel.Error,
            LogSeverity.Warning  => LogLevel.Warning,
            LogSeverity.Info     => LogLevel.Information,
            LogSeverity.Verbose  => LogLevel.Debug,
            LogSeverity.Debug    => LogLevel.Trace,
            _                    => LogLevel.Information
        };
        _logger.Log(level, msg.Exception, "[Discord] {Message}", msg.Message);
        return Task.CompletedTask;
    }
}
