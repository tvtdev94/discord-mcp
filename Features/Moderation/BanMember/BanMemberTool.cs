namespace DiscordMcp.Features.Moderation.BanMember;

[McpServerToolType]
public sealed class BanMemberTool(IMediator mediator)
{
    [McpServerTool(Name = "ban_member"), Description("Ban a member from the Discord server")]
    public Task<string> BanMember(
        [Description("Discord server ID")] string? guildId,
        [Description("User ID to ban")] string userId,
        [Description("Reason for ban")] string? reason = null,
        [Description("Number of days of messages to delete (0-7)")] int deleteMessageDays = 0)
        => mediator.Send(new BanMemberCommand(guildId, userId, reason, deleteMessageDays));
}
