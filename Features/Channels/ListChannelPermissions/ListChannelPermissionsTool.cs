namespace DiscordMcp.Features.Channels.ListChannelPermissions;

[McpServerToolType]
public sealed class ListChannelPermissionsTool(IMediator mediator)
{
    [McpServerTool(Name = "list_channel_permissions"), Description("List all permission overwrites for a channel")]
    public Task<string> ListChannelPermissions(
        [Description("Channel ID")] string channelId)
        => mediator.Send(new ListChannelPermissionsQuery(channelId));
}
