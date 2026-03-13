namespace DiscordMcp.Features.Moderation.KickMember;

[McpServerToolType]
public sealed class KickMemberTool(IMediator mediator)
{
    [McpServerTool(Name = "kick_member"), Description("Kick a member from the Discord server")]
    public Task<string> KickMember(
        [Description("Discord server ID")] string? guildId,
        [Description("User ID to kick")] string userId,
        [Description("Reason for kick")] string? reason = null)
        => mediator.Send(new KickMemberCommand(guildId, userId, reason));
}
