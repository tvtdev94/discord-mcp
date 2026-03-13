namespace DiscordMcp.Features.Emojis.DeleteEmoji;

/// <summary>Command to delete a custom emoji from a Discord server.</summary>
public record DeleteEmojiCommand(string? GuildId, string EmojiId) : IRequest<string>;
