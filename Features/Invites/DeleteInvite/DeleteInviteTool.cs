namespace DiscordMcp.Features.Invites.DeleteInvite;

[McpServerToolType]
public sealed class DeleteInviteTool(IMediator mediator)
{
    [McpServerTool(Name = "delete_invite"), Description("Delete an invite from a Discord guild by its code")]
    public Task<string> DeleteInvite(
        [Description("The invite code to delete (e.g. 'abc123', not the full URL)")] string inviteCode,
        [Description("Guild/server ID (uses DISCORD_GUILD_ID env var if omitted)")] string? guildId = null)
        => mediator.Send(new DeleteInviteCommand(guildId, inviteCode));
}
