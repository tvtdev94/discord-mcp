namespace DiscordMcp.Features.Moderation.UnbanMember;

[McpServerToolType]
public sealed class UnbanMemberTool(IMediator mediator)
{
    [McpServerTool(Name = "unban_member"), Description("Unban a member from the Discord server")]
    public Task<string> UnbanMember(
        [Description("Discord server ID")] string? guildId,
        [Description("User ID to unban")] string userId)
        => mediator.Send(new UnbanMemberCommand(guildId, userId));
}
