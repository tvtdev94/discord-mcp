namespace DiscordMcp.Features.Messages.AddReaction;

/// <summary>Command to add an emoji reaction to a Discord message.</summary>
public record AddReactionCommand(string ChannelId, string MessageId, string Emoji) : IRequest<string>;
