namespace DiscordMcp.Features.Moderation.GetAuditLog;

[McpServerToolType]
public sealed class GetAuditLogTool(IMediator mediator)
{
    [McpServerTool(Name = "get_audit_log"), Description("Retrieve audit log entries from the Discord server")]
    public Task<string> GetAuditLog(
        [Description("Discord server ID")] string? guildId,
        [Description("Number of entries to retrieve (1-100, default 10)")] int limit = 10)
        => mediator.Send(new GetAuditLogQuery(guildId, limit));
}
