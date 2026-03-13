namespace DiscordMcp.Features.Contacts.RemoveContact;

/// <summary>Command to remove a saved contact by name.</summary>
public record RemoveContactCommand(string Name) : IRequest<string>;
