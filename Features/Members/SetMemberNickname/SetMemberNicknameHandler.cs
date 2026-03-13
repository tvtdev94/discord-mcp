namespace DiscordMcp.Features.Members.SetMemberNickname;

public sealed class SetMemberNicknameHandler(DiscordSocketClient client)
    : IRequestHandler<SetMemberNicknameCommand, string>
{
    public async Task<string> Handle(SetMemberNicknameCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId)) throw new ArgumentException("userId cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var member = guild.GetUser(SafeParser.ParseUlong(request.UserId, "userId"))
            ?? throw new ArgumentException($"Member '{request.UserId}' not found in server.");

        // null nickname resets to the default username
        await member.ModifyAsync(props => props.Nickname = request.Nickname);

        return request.Nickname is null
            ? $"Nickname reset for {member.Username}#{member.Discriminator} (ID: {member.Id})."
            : $"Nickname set to '{request.Nickname}' for {member.Username}#{member.Discriminator} (ID: {member.Id}).";
    }
}
