namespace DiscordMcp.Features.Invites.DeleteInvite;

public sealed class DeleteInviteHandler(DiscordSocketClient client)
    : IRequestHandler<DeleteInviteCommand, string>
{
    public async Task<string> Handle(DeleteInviteCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.InviteCode))
            throw new ArgumentException("inviteCode cannot be null or empty.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var invites = await guild.GetInvitesAsync();
        var invite  = invites.FirstOrDefault(i =>
            string.Equals(i.Code, request.InviteCode, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"Invite not found with code: {request.InviteCode}");

        await invite.DeleteAsync();

        return $"Successfully deleted invite: {request.InviteCode}";
    }
}
