using Microsoft.Extensions.Logging;

namespace DiscordMcp.Infrastructure;

/// <summary>
/// Holds the identity of the operator running this MCP instance.
/// Populated from DISCORD_OPERATOR_ID env var on startup via Initialize().
/// If the env var is not set, IsConfigured is false and all attribution/audit
/// features degrade gracefully.
/// </summary>
public sealed class OperatorContext
{
    /// <summary>Discord user ID of the operator, or 0 if not configured.</summary>
    public ulong Id { get; private set; }

    /// <summary>Discord username (e.g. "alice"), or empty string.</summary>
    public string Username { get; private set; } = string.Empty;

    /// <summary>Display name shown in message prefix (e.g. "Alice").</summary>
    public string DisplayName { get; private set; } = string.Empty;

    /// <summary>True when DISCORD_OPERATOR_ID was set and the user was resolved.</summary>
    public bool IsConfigured { get; private set; }

    /// <summary>
    /// Resolves the operator user via the Discord gateway after the client is Ready.
    /// Must be called from DiscordClientHostedService after the Ready event fires.
    /// </summary>
    public async Task InitializeAsync(DiscordSocketClient client, ILogger logger)
    {
        var raw = Environment.GetEnvironmentVariable("DISCORD_OPERATOR_ID");
        if (string.IsNullOrWhiteSpace(raw))
        {
            logger.LogInformation("DISCORD_OPERATOR_ID not set — attribution and audit logging disabled.");
            return;
        }

        if (!ulong.TryParse(raw.Trim(), out var userId))
        {
            logger.LogWarning("DISCORD_OPERATOR_ID '{Raw}' is not a valid Discord snowflake — attribution disabled.", raw);
            return;
        }

        try
        {
            var user = await client.GetUserAsync(userId);
            if (user is null)
            {
                logger.LogWarning("DISCORD_OPERATOR_ID {UserId} could not be resolved to a Discord user — attribution disabled.", userId);
                return;
            }

            Id = userId;
            Username = user.Username;
            DisplayName = string.IsNullOrWhiteSpace(user.GlobalName) ? user.Username : user.GlobalName;
            IsConfigured = true;

            logger.LogInformation("Operator resolved: {DisplayName} ({Username}, {UserId})", DisplayName, Username, userId);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to resolve DISCORD_OPERATOR_ID {UserId} — attribution disabled.", userId);
        }
    }

    /// <summary>Returns the formatted prefix string for message attribution.</summary>
    public string Prefix => $"**[via @{DisplayName}]**";
}
