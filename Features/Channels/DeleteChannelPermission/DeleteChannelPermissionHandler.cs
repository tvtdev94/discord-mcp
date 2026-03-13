namespace DiscordMcp.Features.Channels.DeleteChannelPermission;

public sealed class DeleteChannelPermissionHandler(DiscordSocketClient client)
    : IRequestHandler<DeleteChannelPermissionCommand, string>
{
    public async Task<string> Handle(DeleteChannelPermissionCommand request, CancellationToken cancellationToken)
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

        string targetName;
        if (targetType == "role")
        {
            var role = channel.Guild.GetRole(SafeParser.ParseUlong(request.TargetId, "targetId"))
                ?? throw new ArgumentException("Role not found by targetId.");
            await channel.RemovePermissionOverwriteAsync(role);
            targetName = $"role @{role.Name}";
        }
        else
        {
            var user = channel.Guild.GetUser(SafeParser.ParseUlong(request.TargetId, "targetId"))
                ?? throw new ArgumentException("Guild member not found by targetId.");
            await channel.RemovePermissionOverwriteAsync(user);
            targetName = $"user @{user.Username}";
        }

        return $"Permission overwrite removed for {targetName} on channel #{channel.Name} (ID: {channel.Id}).";
    }
}
