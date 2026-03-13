namespace DiscordMcp.Features.Threads.ArchiveThread;

/// <summary>Command to archive an active Discord thread channel.</summary>
public record ArchiveThreadCommand(string ThreadId) : IRequest<string>;
