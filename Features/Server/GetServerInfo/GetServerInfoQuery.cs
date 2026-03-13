namespace DiscordMcp.Features.Server.GetServerInfo;

/// <summary>Query to retrieve detailed Discord server (guild) information.</summary>
public record GetServerInfoQuery(string? GuildId) : IRequest<string>;
