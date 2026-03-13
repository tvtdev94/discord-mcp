namespace DiscordMcp.Features.Channels.SetChannelPermission;

/// <summary>Command to set a permission overwrite for a role or user on a channel.</summary>
public record SetChannelPermissionCommand(
    string ChannelId,
    string TargetId,
    string TargetType,
    string? AllowPermissions,
    string? DenyPermissions) : IRequest<string>;
