namespace DiscordMcp.Features.Channels.EditChannel;

/// <summary>Command to edit the name and/or topic of a text channel.</summary>
public record EditChannelCommand(string ChannelId, string? Name, string? Topic) : IRequest<string>;
