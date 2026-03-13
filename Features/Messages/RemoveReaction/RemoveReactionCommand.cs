namespace DiscordMcp.Features.Messages.RemoveReaction;

/// <summary>Command to remove an emoji reaction from a Discord message.</summary>
public record RemoveReactionCommand(string ChannelId, string MessageId, string Emoji) : IRequest<string>;
