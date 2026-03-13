namespace DiscordMcp.Features.Channels.EditChannel;

public sealed class EditChannelHandler(DiscordSocketClient client)
    : IRequestHandler<EditChannelCommand, string>
{
    public async Task<string> Handle(EditChannelCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId))
            throw new ArgumentException("channelId cannot be null.");
        if (request.Name is null && request.Topic is null)
            throw new ArgumentException("At least one of name or topic must be provided.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as SocketTextChannel
            ?? throw new ArgumentException("Text channel not found by channelId.");

        await channel.ModifyAsync(x =>
        {
            if (request.Name is not null)  x.Name  = request.Name;
            if (request.Topic is not null) x.Topic = request.Topic;
        });

        return $"Channel updated: #{channel.Name} (ID: {channel.Id}).";
    }
}
