using System.Text.Json;

namespace DiscordMcp.Infrastructure;

/// <summary>
/// Writes append-only JSONL audit entries to daily rotating files.
/// Path: {DISCORD_AUDIT_PATH || AppBaseDir}/audit/{operatorId}/{yyyy-MM-dd}.jsonl
/// One JSON object per line — safe to tail -f, crash-safe, easy to parse with jq.
/// </summary>
public sealed class AuditLogger
{
    private readonly string _baseDir;
    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public AuditLogger()
    {
        _baseDir = Path.Combine(
            Environment.GetEnvironmentVariable("DISCORD_AUDIT_PATH") ?? AppContext.BaseDirectory,
            "audit");
    }

    /// <summary>
    /// Appends one audit entry for the given operator action.
    /// No-op if operatorId is 0 (operator not configured).
    /// </summary>
    public void Write(ulong operatorId, string operatorName, string action, object? parameters, long durationMs)
    {
        if (operatorId == 0) return;

        var entry = new AuditEntry(
            Timestamp: DateTimeOffset.UtcNow.ToString("O"),
            OperatorId: operatorId.ToString(),
            OperatorName: operatorName,
            Action: action,
            Params: parameters,
            DurationMs: durationMs);

        var line = JsonSerializer.Serialize(entry, _jsonOpts);
        var filePath = GetFilePath(operatorId);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.AppendAllText(filePath, line + Environment.NewLine);
        }
        catch
        {
            // Audit must never crash the application — swallow silently.
        }
    }

    private string GetFilePath(ulong operatorId)
    {
        var date = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd");
        return Path.Combine(_baseDir, operatorId.ToString(), $"{date}.jsonl");
    }

    private sealed record AuditEntry(
        string Timestamp,
        string OperatorId,
        string OperatorName,
        string Action,
        object? Params,
        long DurationMs);
}
