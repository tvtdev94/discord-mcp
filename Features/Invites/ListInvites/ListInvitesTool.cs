namespace DiscordMcp.Features.Invites.ListInvites;

[McpServerToolType]
public sealed class ListInvitesTool(IMediator mediator)
{
    [McpServerTool(Name = "list_invites"), Description("List all active invites in a Discord guild")]
    public Task<string> ListInvites(
        [Description("Guild/server ID (uses DISCORD_GUILD_ID env var if omitted)")] string? guildId = null)
        => mediator.Send(new ListInvitesQuery(guildId));
}
