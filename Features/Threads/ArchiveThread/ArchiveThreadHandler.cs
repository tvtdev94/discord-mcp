namespace DiscordMcp.Features.Threads.ArchiveThread;

public sealed class ArchiveThreadHandler(DiscordSocketClient client)
    : IRequestHandler<ArchiveThreadCommand, string>
{
    public async Task<string> Handle(ArchiveThreadCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ThreadId))
            throw new ArgumentException("threadId cannot be null or empty.");

        var thread = client.GetChannel(SafeParser.ParseUlong(request.ThreadId, "threadId")) as SocketThreadChannel
            ?? throw new ArgumentException($"Thread channel not found by threadId: {request.ThreadId}");

        await thread.ModifyAsync(x => x.Archived = true);

        return $"Successfully archived thread: {thread.Name} (ID: {thread.Id}).";
    }
}
