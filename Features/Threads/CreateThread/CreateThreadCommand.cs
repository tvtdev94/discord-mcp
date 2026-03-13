namespace DiscordMcp.Features.Threads.CreateThread;

/// <summary>Command to create a new public thread in a Discord text channel.</summary>
public record CreateThreadCommand(string ChannelId, string Name, string? Message) : IRequest<string>;
