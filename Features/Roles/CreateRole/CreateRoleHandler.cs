using Discord;

namespace DiscordMcp.Features.Roles.CreateRole;

public sealed class CreateRoleHandler(DiscordSocketClient client)
    : IRequestHandler<CreateRoleCommand, string>
{
    public async Task<string> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) throw new ArgumentException("name cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        uint  colorVal       = (uint)SafeParser.ParseUlongOrDefault(request.Color, 0);
        bool  hoistVal       = SafeParser.ParseBoolOrDefault(request.Hoist, false);
        bool  mentionableVal = SafeParser.ParseBoolOrDefault(request.Mentionable, false);
        ulong permVal        = SafeParser.ParseUlongOrDefault(request.Permissions, 0);

        var role = await guild.CreateRoleAsync(request.Name,
            permissions:   new GuildPermissions(permVal),
            color:         colorVal != 0 ? new Color(colorVal) : (Color?)null,
            isHoisted:     hoistVal,
            isMentionable: mentionableVal);

        return $"Successfully created role: **{role.Name}** (ID: {role.Id})\n" +
               $"• Color: {role.Color.RawValue}\n" +
               $"• Hoisted: {role.IsHoisted}\n" +
               $"• Mentionable: {role.IsMentionable}\n" +
               $"• Permissions: {role.Permissions.RawValue}";
    }
}
