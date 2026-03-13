namespace DiscordMcp.Features.Threads.UnarchiveThread;

/// <summary>Command to unarchive a previously archived Discord thread channel.</summary>
public record UnarchiveThreadCommand(string ThreadId) : IRequest<string>;
