namespace DiscordMcp.Features.Channels.CreateTextChannel;

/// <summary>Command to create a new text channel in a Discord server.</summary>
public record CreateTextChannelCommand(string? GuildId, string Name, string? CategoryId) : IRequest<string>;
