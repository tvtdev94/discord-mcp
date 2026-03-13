using Discord;

namespace DiscordMcp.Features.Messages.PinMessage;

public sealed class PinMessageHandler(DiscordSocketClient client)
    : IRequestHandler<PinMessageCommand, string>
{
    public async Task<string> Handle(PinMessageCommand request, CancellationToken cancellationToken)
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
            throw new InvalidOperationException("Only user messages can be pinned.");

        await userMessage.PinAsync();

        return $"Message {message.Id} pinned in channel (ID: {request.ChannelId}).";
    }
}
