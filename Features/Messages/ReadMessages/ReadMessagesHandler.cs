using Discord;

namespace DiscordMcp.Features.Messages.ReadMessages;

public sealed class ReadMessagesHandler(DiscordSocketClient client)
    : IRequestHandler<ReadMessagesQuery, string>
{
    public async Task<string> Handle(ReadMessagesQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId)) throw new ArgumentException("channelId cannot be null.");

        int limit = Math.Clamp(SafeParser.ParseIntOrDefault(request.Count, 100), 1, 100);

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as IMessageChannel
            ?? throw new ArgumentException("Channel not found by channelId.");

        var messages = await channel.GetMessagesAsync(limit).FlattenAsync();
        return MessageFormatter.FormatAll(messages);
    }
}
