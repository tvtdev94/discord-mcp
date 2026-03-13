namespace DiscordMcp.Features.Messages.ListPinnedMessages;

/// <summary>Query to list all pinned messages in a channel.</summary>
public record ListPinnedMessagesQuery(string ChannelId) : IRequest<string>;
