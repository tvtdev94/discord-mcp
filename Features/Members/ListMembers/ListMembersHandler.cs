namespace DiscordMcp.Features.Members.ListMembers;

public sealed class ListMembersHandler(DiscordSocketClient client)
    : IRequestHandler<ListMembersQuery, string>
{
    public Task<string> Handle(ListMembersQuery request, CancellationToken cancellationToken)
    {
        var limit = Math.Clamp(request.Limit, 1, 100);

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var members = guild.Users.Take(limit).ToList();
        if (members.Count == 0)
            return Task.FromResult("No members found.");

        var lines = members.Select(m =>
        {
            var roles = m.Roles
                .Where(r => !r.IsEveryone)
                .Select(r => r.Name);
            var joinedAt = m.JoinedAt.HasValue
                ? m.JoinedAt.Value.UtcDateTime.ToString("yyyy-MM-dd")
                : "unknown";
            return $"• {m.Username}#{m.Discriminator} (ID: {m.Id})" +
                   $"{(m.IsBot ? " [BOT]" : string.Empty)}\n" +
                   $"  Joined: {joinedAt} | Roles: {string.Join(", ", roles)}";
        });

        return Task.FromResult(
            $"Members ({members.Count}{(guild.Users.Count > limit ? $" of {guild.Users.Count}" : string.Empty)}):\n" +
            string.Join("\n", lines));
    }
}
