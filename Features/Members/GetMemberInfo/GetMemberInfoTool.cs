namespace DiscordMcp.Features.Members.GetMemberInfo;

[McpServerToolType]
public sealed class GetMemberInfoTool(IMediator mediator)
{
    [McpServerTool(Name = "get_member_info"), Description("Get detailed information about a specific Discord server member")]
    public Task<string> GetMemberInfo(
        [Description("User ID of the member")] string userId,
        [Description("Discord server ID")] string? guildId = null)
        => mediator.Send(new GetMemberInfoQuery(guildId, userId));
}
