namespace DiscordMcp.Features.Server.EditServer;

/// <summary>Command to edit the name and/or description of a Discord server.</summary>
public record EditServerCommand(string? GuildId, string? Name, string? Description) : IRequest<string>;
