using Discord;

namespace DiscordMcp.Features.Messages.SendMessage;

public sealed class SendMessageHandler(DiscordSocketClient client, OperatorContext operatorContext)
    : IRequestHandler<SendMessageCommand, string>
{
    public async Task<string> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId)) throw new ArgumentException("channelId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.Message))   throw new ArgumentException("message cannot be null.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as IMessageChannel
            ?? throw new ArgumentException("Channel not found by channelId.");

        var content = await MessagePrefixHelper.PrependPrefixIfNeededAsync(channel, client, operatorContext, request.Message);
        var sent = await channel.SendMessageAsync(content);
        return $"Message sent successfully. Message link: {sent.GetJumpUrl()}";
    }
}
