namespace DiscordMcp.Features.Roles.RemoveRole;

public sealed class RemoveRoleHandler(DiscordSocketClient client)
    : IRequestHandler<RemoveRoleCommand, string>
{
    public async Task<string> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId)) throw new ArgumentException("userId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.RoleId)) throw new ArgumentException("roleId cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var member = guild.GetUser(SafeParser.ParseUlong(request.UserId, "userId"))
            ?? throw new ArgumentException("User not found in this server by userId.");

        var role = guild.GetRole(SafeParser.ParseUlong(request.RoleId, "roleId"))
            ?? throw new ArgumentException("Role not found by roleId.");

        if (role.IsEveryone)
            throw new ArgumentException("Cannot remove the @everyone role.");

        await member.RemoveRoleAsync(role);
        return $"Successfully removed role **{role.Name}** from user **{member.Username}** (ID: {member.Id})";
    }
}
