using Discord;

namespace DiscordMcp.Features.Channels.ListChannels;

public sealed class ListChannelsHandler(DiscordSocketClient client)
    : IRequestHandler<ListChannelsQuery, string>
{
    public Task<string> Handle(ListChannelsQuery request, CancellationToken cancellationToken)
    {
        var guild = GuildResolver.Resolve(client, request.GuildId);

        var channels = guild.Channels;
        if (channels.Count == 0)
            throw new ArgumentException("No channels found by guildId.");

        var lines = channels.Select(c => $"- {c.GetChannelType()} channel: {c.Name} (ID: {c.Id})");
        return Task.FromResult($"Retrieved {channels.Count} channels:\n{string.Join("\n", lines)}");
    }
}
