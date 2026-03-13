namespace DiscordMcp.Features.Invites.CreateInvite;

public sealed class CreateInviteHandler(DiscordSocketClient client)
    : IRequestHandler<CreateInviteCommand, string>
{
    public async Task<string> Handle(CreateInviteCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId))
            throw new ArgumentException("channelId cannot be null or empty.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as SocketTextChannel
            ?? throw new ArgumentException($"Text channel not found by channelId: {request.ChannelId}");

        // maxAge: seconds until expiry — 0 = permanent; default 86400 (24 h)
        // maxUses: 0 = unlimited
        var invite = await channel.CreateInviteAsync(
            maxAge:  request.MaxAge  ?? 86400,
            maxUses: request.MaxUses ?? 0);

        return $"Successfully created invite for #{channel.Name}: https://discord.gg/{invite.Code}";
    }
}
