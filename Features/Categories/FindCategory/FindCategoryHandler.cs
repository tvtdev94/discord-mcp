namespace DiscordMcp.Features.Categories.FindCategory;

public sealed class FindCategoryHandler(DiscordSocketClient client)
    : IRequestHandler<FindCategoryQuery, string>
{
    public Task<string> Handle(FindCategoryQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CategoryName)) throw new ArgumentException("categoryName cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var matches = guild.CategoryChannels
            .Where(c => c.Name.Equals(request.CategoryName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matches.Count == 0)
            throw new ArgumentException($"Category '{request.CategoryName}' not found.");

        if (matches.Count > 1)
        {
            var list = matches.Select(c => $"**{c.Name}** - `{c.Id}`");
            throw new ArgumentException(
                $"Multiple categories found with name '{request.CategoryName}'.\n" +
                $"List: {string.Join(", ", list)}.\nPlease specify the category ID.");
        }

        var cat = matches[0];
        return Task.FromResult($"Retrieved category: {cat.Name}, with ID: {cat.Id}");
    }
}
