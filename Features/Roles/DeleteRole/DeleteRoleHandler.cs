namespace DiscordMcp.Features.Roles.DeleteRole;

public sealed class DeleteRoleHandler(DiscordSocketClient client)
    : IRequestHandler<DeleteRoleCommand, string>
{
    public async Task<string> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RoleId)) throw new ArgumentException("roleId cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var role = guild.GetRole(SafeParser.ParseUlong(request.RoleId, "roleId"))
            ?? throw new ArgumentException("Role not found by roleId.");

        if (role.IsEveryone)
            throw new ArgumentException("Cannot delete the @everyone role.");

        string roleName = role.Name;
        await role.DeleteAsync();
        return $"Successfully deleted role: **{roleName}** (ID: {request.RoleId})";
    }
}
