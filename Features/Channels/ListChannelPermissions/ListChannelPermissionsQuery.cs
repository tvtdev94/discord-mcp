namespace DiscordMcp.Features.Channels.ListChannelPermissions;

/// <summary>Query to list all permission overwrites for a channel.</summary>
public record ListChannelPermissionsQuery(string ChannelId) : IRequest<string>;
