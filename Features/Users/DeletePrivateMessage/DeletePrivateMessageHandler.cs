using Discord;

namespace DiscordMcp.Features.Users.DeletePrivateMessage;

public sealed class DeletePrivateMessageHandler(DiscordSocketClient client)
    : IRequestHandler<DeletePrivateMessageCommand, string>
{
    public async Task<string> Handle(DeletePrivateMessageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))    throw new ArgumentException("userId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.MessageId)) throw new ArgumentException("messageId cannot be null.");

        var user = await GetUserByIdAsync(SafeParser.ParseUlong(request.UserId, "userId"))
            ?? throw new ArgumentException("User not found by userId.");

        var dmChannel = await user.CreateDMChannelAsync();
        var msg = await dmChannel.GetMessageAsync(SafeParser.ParseUlong(request.MessageId, "messageId"))
            ?? throw new ArgumentException("Message not found by messageId.");

        await msg.DeleteAsync();
        return "Message deleted successfully.";
    }

    private async Task<IUser?> GetUserByIdAsync(ulong userId)
    {
        var cached = client.GetUser(userId);
        if (cached is not null) return cached;
        try { return await client.GetUserAsync(userId); }
        catch { return null; }
    }
}
