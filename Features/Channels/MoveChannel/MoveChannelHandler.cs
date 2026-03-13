namespace DiscordMcp.Features.Channels.MoveChannel;

public sealed class MoveChannelHandler(DiscordSocketClient client)
    : IRequestHandler<MoveChannelCommand, string>
{
    public async Task<string> Handle(MoveChannelCommand request, CancellationToken cancellationToken)
    {
        if (request.Position < 0)
            throw new ArgumentException("position must be a non-negative integer.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as SocketGuildChannel
            ?? throw new ArgumentException("Guild channel not found by channelId.");

        await channel.ModifyAsync(x => x.Position = request.Position);

        return $"Channel #{channel.Name} (ID: {channel.Id}) moved to position {request.Position}.";
    }
}
