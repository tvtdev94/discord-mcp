namespace DiscordMcp.Features.Categories.FindCategory;

/// <summary>Query to find a category by name in a Discord server.</summary>
public record FindCategoryQuery(string? GuildId, string CategoryName) : IRequest<string>;
