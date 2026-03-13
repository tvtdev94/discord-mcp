namespace DiscordMcp.Features.Moderation.BulkDeleteMessages;

/// <summary>Command to bulk delete messages from a Discord channel.</summary>
public record BulkDeleteMessagesCommand(string ChannelId, int Count) : IRequest<string>;
