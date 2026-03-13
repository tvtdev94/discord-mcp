using Discord;

namespace DiscordMcp.Features.Roles.EditRole;

public sealed class EditRoleHandler(DiscordSocketClient client)
    : IRequestHandler<EditRoleCommand, string>
{
    public async Task<string> Handle(EditRoleCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RoleId)) throw new ArgumentException("roleId cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var role = guild.GetRole(SafeParser.ParseUlong(request.RoleId, "roleId"))
            ?? throw new ArgumentException("Role not found by roleId.");

        if (role.IsEveryone)
            throw new ArgumentException("Cannot edit the @everyone role.");

        await role.ModifyAsync(props =>
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
                props.Name = request.Name;
            if (!string.IsNullOrWhiteSpace(request.Color))
            {
                uint c = (uint)SafeParser.ParseUlongOrDefault(request.Color, 0);
                props.Color = c != 0
                    ? new Optional<Color>(new Color(c))
                    : new Optional<Color>(Color.Default);
            }
            if (!string.IsNullOrWhiteSpace(request.Hoist))
                props.Hoist = SafeParser.ParseBoolOrDefault(request.Hoist, false);
            if (!string.IsNullOrWhiteSpace(request.Mentionable))
                props.Mentionable = SafeParser.ParseBoolOrDefault(request.Mentionable, false);
            if (!string.IsNullOrWhiteSpace(request.Permissions))
                props.Permissions = new Optional<GuildPermissions>(
                    new GuildPermissions(SafeParser.ParseUlong(request.Permissions, "permissions")));
        });

        return $"Successfully updated role: **{role.Name}** (ID: {role.Id})\n" +
               $"• Color: {role.Color.RawValue}\n" +
               $"• Hoisted: {role.IsHoisted}\n" +
               $"• Mentionable: {role.IsMentionable}\n" +
               $"• Permissions: {role.Permissions.RawValue}";
    }
}
