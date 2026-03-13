using Discord;

namespace DiscordMcp.Features.Messages.RemoveReaction;

public sealed class RemoveReactionHandler(DiscordSocketClient client)
    : IRequestHandler<RemoveReactionCommand, string>
{
    public async Task<string> Handle(RemoveReactionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId)) throw new ArgumentException("channelId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.MessageId)) throw new ArgumentException("messageId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.Emoji))     throw new ArgumentException("emoji cannot be null.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as IMessageChannel
            ?? throw new ArgumentException("Channel not found by channelId.");

        var msg = await channel.GetMessageAsync(SafeParser.ParseUlong(request.MessageId, "messageId"))
            ?? throw new ArgumentException("Message not found by messageId.");

        await msg.RemoveReactionAsync(new Emoji(request.Emoji), client.CurrentUser);
        return $"Removed reaction successfully. Message link: {msg.GetJumpUrl()}";
    }
}
