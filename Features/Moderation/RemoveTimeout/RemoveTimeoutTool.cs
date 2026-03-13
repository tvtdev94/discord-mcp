namespace DiscordMcp.Features.Moderation.RemoveTimeout;

[McpServerToolType]
public sealed class RemoveTimeoutTool(IMediator mediator)
{
    [McpServerTool(Name = "remove_timeout"), Description("Remove a timeout from a Discord server member")]
    public Task<string> RemoveTimeout(
        [Description("Discord server ID")] string? guildId,
        [Description("User ID to remove timeout from")] string userId)
        => mediator.Send(new RemoveTimeoutCommand(guildId, userId));
}
