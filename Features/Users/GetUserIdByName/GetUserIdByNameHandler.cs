namespace DiscordMcp.Features.Users.GetUserIdByName;

public sealed class GetUserIdByNameHandler(DiscordSocketClient client)
    : IRequestHandler<GetUserIdByNameQuery, string>
{
    public Task<string> Handle(GetUserIdByNameQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username)) throw new ArgumentException("username cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        // Parse optional discriminator suffix (legacy tag format username#0000)
        string name          = request.Username;
        string? discriminator = null;
        if (request.Username.Contains('#'))
        {
            int idx       = request.Username.LastIndexOf('#');
            name          = request.Username[..idx];
            discriminator = request.Username[(idx + 1)..];
        }

        var members = guild.Users
            .Where(u => u.Username.Equals(name, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (discriminator is not null)
            members = members.Where(u => u.Discriminator == discriminator).ToList();

        if (members.Count == 0)
            throw new ArgumentException($"No user found with username '{request.Username}'.");

        if (members.Count > 1)
        {
            var list = members.Select(m => $"{m.Username}#{m.Discriminator} (ID: {m.Id})");
            throw new ArgumentException(
                $"Multiple users found with username '{request.Username}'. " +
                $"List: {string.Join(", ", list)}. Please specify the full username#discriminator.");
        }

        return Task.FromResult(members[0].Id.ToString());
    }
}
