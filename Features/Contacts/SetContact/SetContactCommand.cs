namespace DiscordMcp.Features.Contacts.SetContact;

/// <summary>Command to save a name → Discord user ID mapping.</summary>
public record SetContactCommand(string Name, string UserId) : IRequest<string>;
