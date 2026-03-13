namespace DiscordMcp.Features.Invites.CreateInvite;

[McpServerToolType]
public sealed class CreateInviteTool(IMediator mediator)
{
    [McpServerTool(Name = "create_invite"), Description("Create an invite link for a Discord text channel")]
    public Task<string> CreateInvite(
        [Description("ID of the text channel to create an invite for")] string channelId,
        [Description("Seconds until the invite expires. 0 = permanent. Defaults to 86400 (24 hours).")] int? maxAge = null,
        [Description("Maximum number of uses. 0 = unlimited. Defaults to 0.")] int? maxUses = null)
        => mediator.Send(new CreateInviteCommand(channelId, maxAge, maxUses));
}
