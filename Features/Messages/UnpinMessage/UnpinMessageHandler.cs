using Discord;

namespace DiscordMcp.Features.Messages.UnpinMessage;

public sealed class UnpinMessageHandler(DiscordSocketClient client)
    : IRequestHandler<UnpinMessageCommand, string>
{
    public async Task<string> Handle(UnpinMessageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId))
            throw new ArgumentException("channelId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.MessageId))
            throw new ArgumentException("messageId cannot be null.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as IMessageChannel
            ?? throw new ArgumentException("Message channel not found by channelId.");

        var message = await channel.GetMessageAsync(SafeParser.ParseUlong(request.MessageId, "messageId"))
            ?? throw new ArgumentException("Message not found by messageId.");

        if (message is not IUserMessage userMessage)
            throw new InvalidOperationException("Only user messages can be unpinned.");

        await userMessage.UnpinAsync();

        return $"Message {message.Id} unpinned in channel (ID: {request.ChannelId}).";
    }
}
