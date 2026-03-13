namespace DiscordMcp.Features.Emojis.DeleteEmoji;

public sealed class DeleteEmojiHandler(DiscordSocketClient client)
    : IRequestHandler<DeleteEmojiCommand, string>
{
    public async Task<string> Handle(DeleteEmojiCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.EmojiId)) throw new ArgumentException("emojiId cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var emote = await guild.GetEmoteAsync(SafeParser.ParseUlong(request.EmojiId, "emojiId"))
            ?? throw new ArgumentException($"Emoji '{request.EmojiId}' not found.");

        var emoteName = emote.Name;
        await guild.DeleteEmoteAsync(emote);

        return $"Emoji deleted: :{emoteName}: (ID: {request.EmojiId})";
    }
}
