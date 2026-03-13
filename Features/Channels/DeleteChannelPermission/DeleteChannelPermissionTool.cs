namespace DiscordMcp.Features.Channels.DeleteChannelPermission;

[McpServerToolType]
public sealed class DeleteChannelPermissionTool(IMediator mediator)
{
    [McpServerTool(Name = "delete_channel_permission"), Description("Remove a permission overwrite for a role or user from a channel")]
    public Task<string> DeleteChannelPermission(
        [Description("Channel ID")] string channelId,
        [Description("Role or user ID whose overwrite should be removed")] string targetId,
        [Description("Target type: 'role' or 'user'")] string targetType)
        => mediator.Send(new DeleteChannelPermissionCommand(channelId, targetId, targetType));
}
