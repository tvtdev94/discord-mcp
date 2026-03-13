using Discord;

namespace DiscordMcp.Features.Moderation.ListBannedMembers;

public sealed class ListBannedMembersHandler(DiscordSocketClient client)
    : IRequestHandler<ListBannedMembersQuery, string>
{
    public async Task<string> Handle(ListBannedMembersQuery request, CancellationToken cancellationToken)
    {
        var guild = GuildResolver.Resolve(client, request.GuildId);

        // Cast to the covariant interface FlattenAsync expects
        var bans    = await ((IGuild)guild).GetBansAsync().FlattenAsync();
        var banList = bans.ToList();

        if (banList.Count == 0)
            return "No banned members found.";

        var lines = banList.Select(b =>
            $"- {b.User.Username} (ID: {b.User.Id})" +
            (string.IsNullOrWhiteSpace(b.Reason) ? "" : $" | Reason: {b.Reason}"));

        return $"Banned members ({banList.Count}):\n{string.Join("\n", lines)}";
    }
}
