namespace DiscordMcp.Features.Messages.AddMultipleReactions;

/// <summary>Command to add multiple emoji reactions to a Discord message.</summary>
public record AddMultipleReactionsCommand(string ChannelId, string MessageId, string Emojis) : IRequest<string>;
