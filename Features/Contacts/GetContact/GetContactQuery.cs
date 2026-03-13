namespace DiscordMcp.Features.Contacts.GetContact;

/// <summary>Query to look up a Discord user ID by saved contact name.</summary>
public record GetContactQuery(string Name) : IRequest<string>;
