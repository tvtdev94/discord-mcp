using Discord;

namespace DiscordMcp.Features.Messages.EditMessage;

public sealed class EditMessageHandler(DiscordSocketClient client)
    : IRequestHandler<EditMessageCommand, string>
{
    public async Task<string> Handle(EditMessageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId))  throw new ArgumentException("channelId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.MessageId))  throw new ArgumentException("messageId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.NewMessage)) throw new ArgumentException("newMessage cannot be null.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as IMessageChannel
            ?? throw new ArgumentException("Channel not found by channelId.");

        var msg = await channel.GetMessageAsync(SafeParser.ParseUlong(request.MessageId, "messageId"))
            ?? throw new ArgumentException("Message not found by messageId.");

        if (msg is not IUserMessage userMessage)
            throw new ArgumentException("Message cannot be edited (not a user message).");

        if (userMessage.Author.Id != client.CurrentUser.Id)
            throw new ArgumentException("Cannot edit messages from other users. Only bot's own messages can be edited.");

        string jumpUrl = userMessage.GetJumpUrl();
        await userMessage.ModifyAsync(m => m.Content = request.NewMessage);
        return $"Message edited successfully. Message link: {jumpUrl}";
    }
}
