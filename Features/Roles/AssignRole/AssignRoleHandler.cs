namespace DiscordMcp.Features.Roles.AssignRole;

public sealed class AssignRoleHandler(DiscordSocketClient client)
    : IRequestHandler<AssignRoleCommand, string>
{
    public async Task<string> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId)) throw new ArgumentException("userId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.RoleId)) throw new ArgumentException("roleId cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var member = guild.GetUser(SafeParser.ParseUlong(request.UserId, "userId"))
            ?? throw new ArgumentException("User not found in this server by userId.");

        var role = guild.GetRole(SafeParser.ParseUlong(request.RoleId, "roleId"))
            ?? throw new ArgumentException("Role not found by roleId.");

        if (role.IsEveryone)
            throw new ArgumentException("Cannot assign the @everyone role.");

        await member.AddRoleAsync(role);
        return $"Successfully assigned role **{role.Name}** to user **{member.Username}** (ID: {member.Id})";
    }
}
