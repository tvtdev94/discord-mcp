namespace DiscordMcp.Features.Roles.ListRoles;

public sealed class ListRolesHandler(DiscordSocketClient client)
    : IRequestHandler<ListRolesQuery, string>
{
    public Task<string> Handle(ListRolesQuery request, CancellationToken cancellationToken)
    {
        var guild = GuildResolver.Resolve(client, request.GuildId);

        var roles = guild.Roles.ToList();
        if (roles.Count == 0)
            return Task.FromResult("No roles found on this server.");

        var lines = roles.Select(r =>
            $"- **{r.Name}** (ID: {r.Id})\n" +
            $"  • Color: #{r.Color.RawValue:X6}\n" +
            $"  • Position: {r.Position}\n" +
            $"  • Hoisted: {r.IsHoisted}\n" +
            $"  • Mentionable: {r.IsMentionable}\n" +
            $"  • Permissions: {r.Permissions.RawValue}");

        return Task.FromResult($"Retrieved {roles.Count} roles:\n{string.Join("\n", lines)}");
    }
}
