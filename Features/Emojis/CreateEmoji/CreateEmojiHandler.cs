using Discord;

namespace DiscordMcp.Features.Emojis.CreateEmoji;

public sealed class CreateEmojiHandler(DiscordSocketClient client)
    : IRequestHandler<CreateEmojiCommand, string>
{
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(10) };

    // Discord emoji size limit: 256 KB
    private const int MaxImageBytes = 256 * 1024;

    public async Task<string> Handle(CreateEmojiCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) throw new ArgumentException("name cannot be null.");
        if (string.IsNullOrWhiteSpace(request.ImageUrl)) throw new ArgumentException("imageUrl cannot be null.");

        // Validate URL: only allow HTTPS to prevent SSRF against internal networks
        if (!Uri.TryCreate(request.ImageUrl, UriKind.Absolute, out var uri) || uri.Scheme != "https")
            throw new ArgumentException("imageUrl must be a valid HTTPS URL.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        await using var stream = await Http.GetStreamAsync(uri, cancellationToken);

        // Buffer with size cap to prevent OOM from malicious URLs
        using var ms = new MemoryStream();
        var buffer = new byte[8192];
        int totalRead = 0, bytesRead;
        while ((bytesRead = await stream.ReadAsync(buffer, cancellationToken)) > 0)
        {
            totalRead += bytesRead;
            if (totalRead > MaxImageBytes)
                throw new ArgumentException($"Image exceeds maximum size of {MaxImageBytes / 1024} KB.");
            ms.Write(buffer, 0, bytesRead);
        }
        ms.Position = 0;

        var image = new Image(ms);
        var emote = await guild.CreateEmoteAsync(request.Name, image);

        return $"Emoji created: :{emote.Name}: (ID: {emote.Id}){(emote.Animated ? " [animated]" : string.Empty)}";
    }
}
