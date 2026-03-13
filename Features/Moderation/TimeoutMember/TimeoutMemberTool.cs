namespace DiscordMcp.Features.Moderation.TimeoutMember;

[McpServerToolType]
public sealed class TimeoutMemberTool(IMediator mediator)
{
    [McpServerTool(Name = "timeout_member"), Description("Apply a timeout to a Discord server member")]
    public Task<string> TimeoutMember(
        [Description("Discord server ID")] string? guildId,
        [Description("User ID to timeout")] string userId,
        [Description("Timeout duration in minutes")] int durationMinutes,
        [Description("Reason for timeout")] string? reason = null)
        => mediator.Send(new TimeoutMemberCommand(guildId, userId, durationMinutes, reason));
}
