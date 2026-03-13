using Discord;

namespace DiscordMcp.Features.Users.EditPrivateMessage;

public sealed class EditPrivateMessageHandler(DiscordSocketClient client)
    : IRequestHandler<EditPrivateMessageCommand, string>
{
    public async Task<string> Handle(EditPrivateMessageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))     throw new ArgumentException("userId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.MessageId))  throw new ArgumentException("messageId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.NewMessage)) throw new ArgumentException("newMessage cannot be null.");

        var user = await GetUserByIdAsync(SafeParser.ParseUlong(request.UserId, "userId"))
            ?? throw new ArgumentException("User not found by userId.");

        var dmChannel = await user.CreateDMChannelAsync();
        var msg = await dmChannel.GetMessageAsync(SafeParser.ParseUlong(request.MessageId, "messageId"))
            ?? throw new ArgumentException("Message not found by messageId.");

        if (msg is not IUserMessage userMessage)
            throw new ArgumentException("Message cannot be edited (not a user message).");

        if (userMessage.Author.Id != client.CurrentUser.Id)
            throw new ArgumentException("Cannot edit messages from other users. Only bot's own messages can be edited.");

        string jumpUrl = userMessage.GetJumpUrl();
        await userMessage.ModifyAsync(m => m.Content = request.NewMessage);
        return $"Message edited successfully. Message link: {jumpUrl}";
    }

    private async Task<IUser?> GetUserByIdAsync(ulong userId)
    {
        var cached = client.GetUser(userId);
        if (cached is not null) return cached;
        try { return await client.GetUserAsync(userId); }
        catch { return null; }
    }
}
