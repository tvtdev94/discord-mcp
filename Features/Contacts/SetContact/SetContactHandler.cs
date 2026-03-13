namespace DiscordMcp.Features.Contacts.SetContact;

public sealed class SetContactHandler(ContactStore store)
    : IRequestHandler<SetContactCommand, string>
{
    public Task<string> Handle(SetContactCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name cannot be empty.");
        // Validates format and throws a descriptive ArgumentException on bad input
        SafeParser.ParseUlong(request.UserId, "UserId");

        store.Set(request.Name, request.UserId);
        return Task.FromResult($"Contact saved: \"{request.Name.Trim()}\" → {request.UserId.Trim()}");
    }
}
