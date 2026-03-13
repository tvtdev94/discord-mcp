namespace DiscordMcp.Features.Threads.ListArchivedThreads;

/// <summary>Query to list all publicly archived threads in a Discord text channel.</summary>
public record ListArchivedThreadsQuery(string ChannelId) : IRequest<string>;
