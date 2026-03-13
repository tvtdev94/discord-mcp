using Discord;
using System.Text;

namespace DiscordMcp.Features.Channels.ListChannelPermissions;

public sealed class ListChannelPermissionsHandler(DiscordSocketClient client)
    : IRequestHandler<ListChannelPermissionsQuery, string>
{
    public Task<string> Handle(ListChannelPermissionsQuery request, CancellationToken cancellationToken)
    {
        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as SocketGuildChannel
            ?? throw new ArgumentException("Guild channel not found by channelId.");

        var overwrites = channel.PermissionOverwrites;
        if (!overwrites.Any())
            return Task.FromResult($"No permission overwrites found for channel #{channel.Name}.");

        var sb = new StringBuilder();
        sb.AppendLine($"Permission overwrites for #{channel.Name} (ID: {channel.Id}):");

        foreach (var overwrite in overwrites)
        {
            var targetType = overwrite.TargetType == PermissionTarget.Role ? "Role" : "User";
            var targetId   = overwrite.TargetId;
            string targetName;

            if (overwrite.TargetType == PermissionTarget.Role)
            {
                var role = channel.Guild.GetRole(targetId);
                targetName = role is not null ? $"@{role.Name}" : targetId.ToString();
            }
            else
            {
                var user = channel.Guild.GetUser(targetId);
                targetName = user is not null ? $"@{user.Username}" : targetId.ToString();
            }

            sb.AppendLine($"  [{targetType}] {targetName} (ID: {targetId})");
            sb.AppendLine($"    Allow: {overwrite.Permissions.AllowValue}");
            sb.AppendLine($"    Deny:  {overwrite.Permissions.DenyValue}");
        }

        return Task.FromResult(sb.ToString().TrimEnd());
    }
}
