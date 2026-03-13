using Discord;

namespace DiscordMcp.Features.Categories.ListChannelsInCategory;

public sealed class ListChannelsInCategoryHandler(DiscordSocketClient client)
    : IRequestHandler<ListChannelsInCategoryQuery, string>
{
    public Task<string> Handle(ListChannelsInCategoryQuery request, CancellationToken cancellationToken)
    {
        var guild    = GuildResolver.Resolve(client, request.GuildId);
        var category = guild.GetCategoryChannel(SafeParser.ParseUlong(request.CategoryId, "categoryId"))
            ?? throw new ArgumentException("Category not found by categoryId.");

        var channels = category.Channels;
        if (channels.Count == 0)
            throw new ArgumentException("Category does not contain any channels.");

        var lines = channels.Select(c => $"- {c.GetChannelType()} channel: {c.Name} (ID: {c.Id})");
        return Task.FromResult($"Retrieved {channels.Count} channels:\n{string.Join("\n", lines)}");
    }
}
