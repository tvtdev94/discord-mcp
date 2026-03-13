namespace DiscordMcp.Features.Categories.DeleteCategory;

/// <summary>Command to delete a category from a Discord server.</summary>
public record DeleteCategoryCommand(string? GuildId, string CategoryId) : IRequest<string>;
