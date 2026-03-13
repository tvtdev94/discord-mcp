namespace DiscordMcp.Features.Categories.ListChannelsInCategory;

/// <summary>Query to list all channels within a specific category.</summary>
public record ListChannelsInCategoryQuery(string? GuildId, string CategoryId) : IRequest<string>;
