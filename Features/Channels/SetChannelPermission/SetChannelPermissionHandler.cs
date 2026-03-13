using Discord;

namespace DiscordMcp.Features.Channels.SetChannelPermission;

public sealed class SetChannelPermissionHandler(DiscordSocketClient client)
    : IRequestHandler<SetChannelPermissionCommand, string>
{
    public async Task<string> Handle(SetChannelPermissionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.TargetId))
            throw new ArgumentException("targetId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.TargetType))
            throw new ArgumentException("targetType cannot be null.");

        var targetType = request.TargetType.Trim().ToLowerInvariant();
        if (targetType is not ("role" or "user"))
            throw new ArgumentException("targetType must be 'role' or 'user'.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as SocketGuildChannel
            ?? throw new ArgumentException("Guild channel not found by channelId.");

        var allowPerms = ParsePermissions(request.AllowPermissions);
        var denyPerms  = ParsePermissions(request.DenyPermissions);
        var overwrite  = new OverwritePermissions(allowPerms, denyPerms);

        string targetName;
        if (targetType == "role")
        {
            var role = channel.Guild.GetRole(SafeParser.ParseUlong(request.TargetId, "targetId"))
                ?? throw new ArgumentException("Role not found by targetId.");
            await channel.AddPermissionOverwriteAsync(role, overwrite);
            targetName = $"role @{role.Name}";
        }
        else
        {
            var user = channel.Guild.GetUser(SafeParser.ParseUlong(request.TargetId, "targetId"))
                ?? throw new ArgumentException("Guild member not found by targetId.");
            await channel.AddPermissionOverwriteAsync(user, overwrite);
            targetName = $"user @{user.Username}";
        }

        return $"Permission overwrite set for {targetName} on channel #{channel.Name} (ID: {channel.Id}).";
    }

    /// <summary>
    /// Parses a comma-separated list of ChannelPermission names into a combined ulong bitmask.
    /// Throws on unrecognized permission names to prevent silent no-op overwrites.
    /// </summary>
    private static ulong ParsePermissions(string? permissions)
    {
        if (string.IsNullOrWhiteSpace(permissions)) return 0ul;

        ulong mask = 0ul;
        var unknown = new List<string>();
        foreach (var part in permissions.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (Enum.TryParse<ChannelPermission>(part, ignoreCase: true, out var perm))
                mask |= (ulong)perm;
            else
                unknown.Add(part);
        }

        if (unknown.Count > 0)
            throw new ArgumentException($"Unknown permission name(s): {string.Join(", ", unknown)}. " +
                $"Valid values: {string.Join(", ", Enum.GetNames<ChannelPermission>())}");

        return mask;
    }
}
