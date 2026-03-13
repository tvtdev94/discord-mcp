namespace DiscordMcp.Features.Contacts.GetContact;

public sealed class GetContactHandler(ContactStore store)
    : IRequestHandler<GetContactQuery, string>
{
    public Task<string> Handle(GetContactQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name cannot be empty.");

        return store.TryGet(request.Name, out var userId)
            ? Task.FromResult($"{request.Name.Trim()} → {userId}")
            : Task.FromResult($"Contact \"{request.Name.Trim()}\" not found.");
    }
}
