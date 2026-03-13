namespace DiscordMcp.Features.Members.GetMemberInfo;

public sealed class GetMemberInfoHandler(DiscordSocketClient client)
    : IRequestHandler<GetMemberInfoQuery, string>
{
    public Task<string> Handle(GetMemberInfoQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId)) throw new ArgumentException("userId cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var member = guild.GetUser(SafeParser.ParseUlong(request.UserId, "userId"))
            ?? throw new ArgumentException($"Member '{request.UserId}' not found in server.");

        var roles = member.Roles
            .Where(r => !r.IsEveryone)
            .Select(r => r.Name);

        var joinedAt = member.JoinedAt.HasValue
            ? member.JoinedAt.Value.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss UTC")
            : "unknown";

        return Task.FromResult(
            $"**{member.Username}#{member.Discriminator}**\n" +
            $"• ID: {member.Id}\n" +
            $"• Nickname: {member.Nickname ?? "(none)"}\n" +
            $"• Bot: {member.IsBot}\n" +
            $"• Joined: {joinedAt}\n" +
            $"• Roles: {(roles.Any() ? string.Join(", ", roles) : "(none)")}");
    }
}
