namespace DiscordMcp.Features.Threads.UnarchiveThread;

public sealed class UnarchiveThreadHandler(DiscordSocketClient client)
    : IRequestHandler<UnarchiveThreadCommand, string>
{
    public async Task<string> Handle(UnarchiveThreadCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ThreadId))
            throw new ArgumentException("threadId cannot be null or empty.");

        var thread = client.GetChannel(SafeParser.ParseUlong(request.ThreadId, "threadId")) as SocketThreadChannel
            ?? throw new ArgumentException($"Thread channel not found by threadId: {request.ThreadId}");

        await thread.ModifyAsync(x => x.Archived = false);

        return $"Successfully unarchived thread: {thread.Name} (ID: {thread.Id}).";
    }
}
