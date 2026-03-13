namespace DiscordMcp.Features.Channels.SetSlowmode;

public sealed class SetSlowmodeHandler(DiscordSocketClient client)
    : IRequestHandler<SetSlowmodeCommand, string>
{
    private const int MaxSlowmodeSeconds = 21600;

    public async Task<string> Handle(SetSlowmodeCommand request, CancellationToken cancellationToken)
    {
        if (request.Seconds < 0 || request.Seconds > MaxSlowmodeSeconds)
            throw new ArgumentException($"seconds must be between 0 and {MaxSlowmodeSeconds}.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as SocketTextChannel
            ?? throw new ArgumentException("Text channel not found by channelId.");

        await channel.ModifyAsync(x => x.SlowModeInterval = request.Seconds);

        var status = request.Seconds == 0
            ? "disabled"
            : $"set to {request.Seconds} second(s)";

        return $"Slowmode {status} for channel #{channel.Name} (ID: {channel.Id}).";
    }
}
