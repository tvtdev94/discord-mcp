namespace DiscordMcp.Features.Channels.SetChannelPermission;

[McpServerToolType]
public sealed class SetChannelPermissionTool(IMediator mediator)
{
    [McpServerTool(Name = "set_channel_permission"), Description("Set a permission overwrite for a role or user on a channel")]
    public Task<string> SetChannelPermission(
        [Description("Channel ID")] string channelId,
        [Description("Role or user ID to apply the overwrite to")] string targetId,
        [Description("Target type: 'role' or 'user'")] string targetType,
        [Description("Comma-separated permissions to allow (e.g. 'SendMessages,ViewChannel'). Omit or leave empty to allow none.")] string? allowPermissions = null,
        [Description("Comma-separated permissions to deny (e.g. 'SendMessages,ViewChannel'). Omit or leave empty to deny none.")] string? denyPermissions = null)
        => mediator.Send(new SetChannelPermissionCommand(channelId, targetId, targetType, allowPermissions, denyPermissions));
}
