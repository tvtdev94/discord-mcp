namespace DiscordMcp.Features.Members.SetMemberNickname;

[McpServerToolType]
public sealed class SetMemberNicknameTool(IMediator mediator)
{
    [McpServerTool(Name = "set_member_nickname"), Description("Set or reset a member's nickname in a Discord server")]
    public Task<string> SetMemberNickname(
        [Description("User ID of the member")] string userId,
        [Description("New nickname (omit or null to reset to username)")] string? nickname = null,
        [Description("Discord server ID")] string? guildId = null)
        => mediator.Send(new SetMemberNicknameCommand(guildId, userId, nickname));
}
