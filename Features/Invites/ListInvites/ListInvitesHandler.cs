namespace DiscordMcp.Features.Invites.ListInvites;

public sealed class ListInvitesHandler(DiscordSocketClient client)
    : IRequestHandler<ListInvitesQuery, string>
{
    public async Task<string> Handle(ListInvitesQuery request, CancellationToken cancellationToken)
    {
        var guild   = GuildResolver.Resolve(client, request.GuildId);
        var invites = await guild.GetInvitesAsync();
        var list    = invites.ToList();

        if (list.Count == 0)
            return "No active invites found in the server.";

        var lines = list.Select(i =>
        {
            // Uses and MaxUses are nullable ints on IInviteMetadata
            var usesStr  = i is Discord.IInviteMetadata meta
                ? $"{meta.Uses?.ToString() ?? "?"}/{(meta.MaxUses == 0 ? "∞" : meta.MaxUses?.ToString() ?? "?")} uses"
                : "? uses";
            // ExpiresAt is available on IInvite directly
            var expiry  = i.ExpiresAt.HasValue ? $"expires {i.ExpiresAt.Value:u}" : "never expires";
            var channel = i.ChannelName ?? "unknown";
            return $"- {i.Code} | #{channel} | {usesStr} | {expiry}";
        });

        return $"Retrieved {list.Count} invite(s):\n{string.Join("\n", lines)}";
    }
}
