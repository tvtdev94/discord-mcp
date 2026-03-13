using Discord;

namespace DiscordMcp.Infrastructure;

/// <summary>
/// Shared formatting helpers for Discord messages.
/// </summary>
public static class MessageFormatter
{
    private const int MaxContentLength = 2000;

    /// <summary>
    /// Sanitizes raw Discord message content to prevent prompt injection.
    /// Escapes triple-backtick sequences and truncates to Discord's 2000-char limit.
    /// </summary>
    public static string SanitizeContent(string content)
    {
        if (string.IsNullOrEmpty(content))
            return content ?? string.Empty;

        // Escape triple backticks to prevent code block breakout
        var sanitized = content.Replace("```", "\\`\\`\\`");

        // Truncate to Discord message limit
        return sanitized.Length > MaxContentLength
            ? sanitized[..MaxContentLength]
            : sanitized;
    }

    /// <summary>
    /// Formats a single Discord message for text output.
    /// Content is wrapped in [DISCORD_CONTENT] delimiters and sanitized
    /// to help the LLM distinguish tool output from user-supplied content.
    /// </summary>
    public static string Format(IMessage m)
    {
        var sanitized = SanitizeContent(m.Content ?? string.Empty);
        return $"- (ID: {m.Id}) [{m.Author.Username}] `{m.CreatedAt:u}`: [DISCORD_CONTENT]{sanitized}[/DISCORD_CONTENT]";
    }

    /// <summary>
    /// Formats a collection of messages with a count header.
    /// </summary>
    public static string FormatAll(IEnumerable<IMessage> messages)
    {
        var list = messages.ToList();
        var formatted = list.Select(Format);
        return $"**Retrieved {list.Count} messages:**\n{string.Join("\n", formatted)}";
    }
}
