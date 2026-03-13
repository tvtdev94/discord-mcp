using Discord;

namespace DiscordMcp.Infrastructure;

/// <summary>
/// Decides whether to prepend an operator attribution prefix to an outgoing message.
/// Reads the last message in the channel: if it was sent by the bot and already carries
/// this operator's prefix, the prefix is omitted to avoid consecutive duplication.
/// </summary>
public static class MessagePrefixHelper
{
    /// <summary>
    /// Returns the message content to send, with prefix prepended when needed.
    /// </summary>
    /// <param name="channel">Target channel — used to read the last message.</param>
    /// <param name="client">Discord client — used to identify the bot's own user ID.</param>
    /// <param name="operatorCtx">Operator identity; if not configured, returns <paramref name="message"/> unchanged.</param>
    /// <param name="message">The raw message content to send.</param>
    public static async Task<string> PrependPrefixIfNeededAsync(
        IMessageChannel channel,
        DiscordSocketClient client,
        OperatorContext operatorCtx,
        string message)
    {
        if (!operatorCtx.IsConfigured)
            return message;

        var prefix = operatorCtx.Prefix;

        try
        {
            // Fetch the single most-recent message in the channel.
            var recent = await channel.GetMessagesAsync(1).FlattenAsync();
            var last = recent.FirstOrDefault();

            if (last is not null
                && last.Author.Id == client.CurrentUser.Id
                && last.Content.StartsWith(prefix, StringComparison.Ordinal))
            {
                // Same operator sent the previous message — skip prefix.
                return message;
            }
        }
        catch
        {
            // If message history is unavailable (e.g. DM, permissions) just add the prefix.
        }

        return $"{prefix} {message}";
    }
}
