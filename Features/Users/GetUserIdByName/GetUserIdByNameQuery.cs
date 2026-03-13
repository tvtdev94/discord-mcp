namespace DiscordMcp.Features.Users.GetUserIdByName;

/// <summary>Query to look up a user's ID by username within a guild.</summary>
public record GetUserIdByNameQuery(string Username, string? GuildId) : IRequest<string>;
