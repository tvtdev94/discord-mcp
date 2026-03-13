namespace DiscordMcp.Features.Messages.EditMessage;

/// <summary>Command to edit an existing message in a Discord channel.</summary>
public record EditMessageCommand(string ChannelId, string MessageId, string NewMessage) : IRequest<string>;
