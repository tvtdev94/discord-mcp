namespace DiscordMcp.Features.Moderation.ListBannedMembers;

[McpServerToolType]
public sealed class ListBannedMembersTool(IMediator mediator)
{
    [McpServerTool(Name = "list_banned_members"), Description("List all banned members from the Discord server")]
    public Task<string> ListBannedMembers(
        [Description("Discord server ID")] string? guildId)
        => mediator.Send(new ListBannedMembersQuery(guildId));
}
