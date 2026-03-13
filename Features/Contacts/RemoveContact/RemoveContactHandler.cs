namespace DiscordMcp.Features.Contacts.RemoveContact;

public sealed class RemoveContactHandler(ContactStore store)
    : IRequestHandler<RemoveContactCommand, string>
{
    public Task<string> Handle(RemoveContactCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name cannot be empty.");

        return store.Remove(request.Name)
            ? Task.FromResult($"Contact \"{request.Name.Trim()}\" removed.")
            : Task.FromResult($"Contact \"{request.Name.Trim()}\" not found.");
    }
}
