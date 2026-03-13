using Discord;

namespace DiscordMcp.Features.Users.ReadPrivateMessages;

public sealed class ReadPrivateMessagesHandler(DiscordSocketClient client)
    : IRequestHandler<ReadPrivateMessagesQuery, string>
{
    public async Task<string> Handle(ReadPrivateMessagesQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId)) throw new ArgumentException("userId cannot be null.");

        int limit = Math.Clamp(SafeParser.ParseIntOrDefault(request.Count, 100), 1, 100);

        var user = await GetUserByIdAsync(SafeParser.ParseUlong(request.UserId, "userId"))
            ?? throw new ArgumentException("User not found by userId.");

        var dmChannel = await user.CreateDMChannelAsync();
        var messages  = await dmChannel.GetMessagesAsync(limit).FlattenAsync();
        return MessageFormatter.FormatAll(messages);
    }

    private async Task<IUser?> GetUserByIdAsync(ulong userId)
    {
        var cached = client.GetUser(userId);
        if (cached is not null) return cached;
        try { return await client.GetUserAsync(userId); }
        catch { return null; }
    }
}
