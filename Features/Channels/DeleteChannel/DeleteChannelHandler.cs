using Discord;

namespace DiscordMcp.Features.Channels.DeleteChannel;

public sealed class DeleteChannelHandler(DiscordSocketClient client)
    : IRequestHandler<DeleteChannelCommand, string>
{
    public async Task<string> Handle(DeleteChannelCommand request, CancellationToken cancellationToken)
    {
        var guild   = GuildResolver.Resolve(client, request.GuildId);
        var channel = guild.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId"))
            ?? throw new ArgumentException("Channel not found by channelId.");

        await channel.DeleteAsync();
        return $"Deleted {channel.GetChannelType()} channel: {channel.Name}";
    }
}
