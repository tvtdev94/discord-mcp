namespace DiscordMcp.Features.Emojis.ListEmojis;

public sealed class ListEmojisHandler(DiscordSocketClient client)
    : IRequestHandler<ListEmojisQuery, string>
{
    public Task<string> Handle(ListEmojisQuery request, CancellationToken cancellationToken)
    {
        var guild = GuildResolver.Resolve(client, request.GuildId);

        var emotes = guild.Emotes;
        if (emotes.Count == 0)
            return Task.FromResult("No custom emojis found.");

        var lines = emotes.Select(e =>
            $"• :{e.Name}: (ID: {e.Id}){(e.Animated ? " [animated]" : string.Empty)}");

        return Task.FromResult($"Custom Emojis ({emotes.Count}):\n{string.Join("\n", lines)}");
    }
}
