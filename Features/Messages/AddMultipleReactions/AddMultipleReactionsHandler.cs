using Discord;

namespace DiscordMcp.Features.Messages.AddMultipleReactions;

public sealed class AddMultipleReactionsHandler(DiscordSocketClient client)
    : IRequestHandler<AddMultipleReactionsCommand, string>
{
    public async Task<string> Handle(AddMultipleReactionsCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId)) throw new ArgumentException("channelId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.MessageId)) throw new ArgumentException("messageId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.Emojis))    throw new ArgumentException("emojis cannot be null.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as IMessageChannel
            ?? throw new ArgumentException("Channel not found by channelId.");

        var msg = await channel.GetMessageAsync(SafeParser.ParseUlong(request.MessageId, "messageId"))
            ?? throw new ArgumentException("Message not found by messageId.");

        var emojiList = request.Emojis.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var e in emojiList)
        {
            await msg.AddReactionAsync(new Emoji(e));
            // Small delay to respect Discord rate limits on reaction endpoints
            await Task.Delay(300, cancellationToken);
        }

        return $"Added {emojiList.Length} reactions successfully. Message link: {msg.GetJumpUrl()}";
    }
}
