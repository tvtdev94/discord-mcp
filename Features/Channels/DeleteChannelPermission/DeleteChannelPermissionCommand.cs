namespace DiscordMcp.Features.Channels.DeleteChannelPermission;

/// <summary>Command to remove a permission overwrite for a role or user from a channel.</summary>
public record DeleteChannelPermissionCommand(
    string ChannelId,
    string TargetId,
    string TargetType) : IRequest<string>;
