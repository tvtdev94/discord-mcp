using Discord;

namespace DiscordMcp.Features.Channels.FindChannel;

public sealed class FindChannelHandler(DiscordSocketClient client)
    : IRequestHandler<FindChannelQuery, string>
{
    public Task<string> Handle(FindChannelQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelName)) throw new ArgumentException("channelName cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var channels = guild.Channels;
        if (channels.Count == 0)
            throw new ArgumentException("No channels found by guildId.");

        var matched = channels
            .Where(c => c.Name.Equals(request.ChannelName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matched.Count == 0)
            throw new ArgumentException($"No channels found with name {request.ChannelName}.");

        if (matched.Count > 1)
        {
            var list = matched.Select(c => $"- {c.GetChannelType()} channel: {c.Name} (ID: {c.Id})");
            return Task.FromResult($"Retrieved {matched.Count} channels:\n{string.Join("\n", list)}");
        }

        var ch = matched[0];
        return Task.FromResult($"Retrieved {ch.GetChannelType()} channel: {ch.Name} (ID: {ch.Id})");
    }
}
