namespace DiscordMcp.Features.Threads.ListActiveThreads;

/// <summary>Query to list all active threads in a Discord server.</summary>
public record ListActiveThreadsQuery(string? GuildId) : IRequest<string>;
