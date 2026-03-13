using Discord;

namespace DiscordMcp.Features.Moderation.GetAuditLog;

public sealed class GetAuditLogHandler(DiscordSocketClient client)
    : IRequestHandler<GetAuditLogQuery, string>
{
    public async Task<string> Handle(GetAuditLogQuery request, CancellationToken cancellationToken)
    {
        var limit = Math.Clamp(request.Limit, 1, 100);

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var entries = await guild.GetAuditLogsAsync(limit).FlattenAsync();
        var list = entries.ToList();

        if (list.Count == 0)
            return "No audit log entries found.";

        var lines = list.Select(e =>
            $"- [{e.CreatedAt:yyyy-MM-dd HH:mm:ss}] {e.Action} | User: {e.User?.Username ?? "unknown"} (ID: {e.User?.Id})" +
            (string.IsNullOrWhiteSpace(e.Reason) ? "" : $" | Reason: {e.Reason}"));

        return $"Audit log entries ({list.Count}):\n{string.Join("\n", lines)}";
    }
}
