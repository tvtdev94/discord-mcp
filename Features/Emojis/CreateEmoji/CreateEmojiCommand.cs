namespace DiscordMcp.Features.Emojis.CreateEmoji;

/// <summary>Command to create a custom emoji in a Discord server from an image URL.</summary>
public record CreateEmojiCommand(string? GuildId, string Name, string ImageUrl) : IRequest<string>;
