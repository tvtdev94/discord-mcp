using Discord;

namespace DiscordMcp.Features.Threads.CreateThread;

public sealed class CreateThreadHandler(DiscordSocketClient client)
    : IRequestHandler<CreateThreadCommand, string>
{
    public async Task<string> Handle(CreateThreadCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId)) throw new ArgumentException("channelId cannot be null or empty.");
        if (string.IsNullOrWhiteSpace(request.Name))      throw new ArgumentException("name cannot be null or empty.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as SocketTextChannel
            ?? throw new ArgumentException($"Text channel not found by channelId: {request.ChannelId}");

        var thread = await channel.CreateThreadAsync(
            name:                request.Name,
            type:                ThreadType.PublicThread,
            autoArchiveDuration: ThreadArchiveDuration.OneDay);

        if (!string.IsNullOrWhiteSpace(request.Message))
            await thread.SendMessageAsync(request.Message);

        return $"Successfully created thread: {thread.Name} (ID: {thread.Id}) in #{channel.Name}";
    }
}
