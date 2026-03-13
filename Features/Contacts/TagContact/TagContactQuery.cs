namespace DiscordMcp.Features.Contacts.TagContact;

/// <summary>Query to tag a saved contact by name in a Discord channel.</summary>
public record TagContactQuery(string Name, string ChannelId, string? Message) : IRequest<string>;
