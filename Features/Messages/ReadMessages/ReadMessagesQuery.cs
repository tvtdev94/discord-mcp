namespace DiscordMcp.Features.Messages.ReadMessages;

/// <summary>Query to read recent message history from a Discord channel.</summary>
public record ReadMessagesQuery(string ChannelId, string? Count) : IRequest<string>;
