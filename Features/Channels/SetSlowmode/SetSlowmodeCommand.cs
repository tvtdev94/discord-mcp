namespace DiscordMcp.Features.Channels.SetSlowmode;

/// <summary>Command to set the slowmode interval for a text channel.</summary>
public record SetSlowmodeCommand(string ChannelId, int Seconds) : IRequest<string>;
