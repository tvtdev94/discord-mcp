namespace DiscordMcp.Features.Members.ListMembers;

[McpServerToolType]
public sealed class ListMembersTool(IMediator mediator)
{
    [McpServerTool(Name = "list_members"), Description("List members of a Discord server (max 100)")]
    public Task<string> ListMembers(
        [Description("Discord server ID")] string? guildId = null,
        [Description("Number of members to return (1-100, default 20)")] int limit = 20)
        => mediator.Send(new ListMembersQuery(guildId, limit));
}
