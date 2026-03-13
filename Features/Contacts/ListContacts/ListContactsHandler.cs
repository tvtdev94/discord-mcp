namespace DiscordMcp.Features.Contacts.ListContacts;

public sealed class ListContactsHandler(ContactStore store)
    : IRequestHandler<ListContactsQuery, string>
{
    public Task<string> Handle(ListContactsQuery request, CancellationToken cancellationToken)
    {
        var all = store.GetAll();
        if (all.Count == 0)
            return Task.FromResult("No contacts saved.");

        var lines = all.Select(kv => $"- {kv.Key} → {kv.Value}");
        return Task.FromResult($"Contacts ({all.Count}):\n{string.Join('\n', lines)}");
    }
}
