using Discord;

namespace DiscordMcp.Features.Users.SendPrivateMessage;

public sealed class SendPrivateMessageHandler(DiscordSocketClient client)
    : IRequestHandler<SendPrivateMessageCommand, string>
{
    public async Task<string> Handle(SendPrivateMessageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))  throw new ArgumentException("userId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.Message)) throw new ArgumentException("message cannot be null.");

        var user = await GetUserByIdAsync(SafeParser.ParseUlong(request.UserId, "userId"))
            ?? throw new ArgumentException("User not found by userId.");

        var dmChannel = await user.CreateDMChannelAsync();
        var sent = await dmChannel.SendMessageAsync(request.Message);
        return $"Message sent successfully. Message link: {sent.GetJumpUrl()}";
    }

    private async Task<IUser?> GetUserByIdAsync(ulong userId)
    {
        var cached = client.GetUser(userId);
        if (cached is not null) return cached;
        try { return await client.GetUserAsync(userId); }
        catch { return null; }
    }
}
