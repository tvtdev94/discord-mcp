namespace DiscordMcp.Features.Categories.CreateCategory;

/// <summary>Command to create a new category channel in a Discord server.</summary>
public record CreateCategoryCommand(string? GuildId, string Name) : IRequest<string>;
