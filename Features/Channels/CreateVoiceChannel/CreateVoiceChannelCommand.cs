namespace DiscordMcp.Features.Channels.CreateVoiceChannel;

/// <summary>Command to create a new voice channel in a Discord server.</summary>
public record CreateVoiceChannelCommand(string? GuildId, string Name, string? CategoryId) : IRequest<string>;
