namespace DiscordMcp.Infrastructure;

/// <summary>
/// Safe parsing helpers that throw descriptive ArgumentExceptions
/// instead of raw FormatExceptions when user input is invalid.
/// </summary>
public static class SafeParser
{
    public static ulong ParseUlong(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} cannot be null or empty.");
        if (!ulong.TryParse(value.Trim(), out var result))
            throw new ArgumentException($"{paramName} '{value}' is not a valid Discord snowflake ID.");
        return result;
    }

    public static ulong ParseUlongOrDefault(string? value, ulong defaultValue) =>
        string.IsNullOrWhiteSpace(value) ? defaultValue : ulong.TryParse(value.Trim(), out var r) ? r : defaultValue;

    public static int ParseInt(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} cannot be null or empty.");
        if (!int.TryParse(value.Trim(), out var result))
            throw new ArgumentException($"{paramName} '{value}' is not a valid integer.");
        return result;
    }

    public static int ParseIntOrDefault(string? value, int defaultValue) =>
        string.IsNullOrWhiteSpace(value) ? defaultValue : int.TryParse(value.Trim(), out var r) ? r : defaultValue;

    public static bool ParseBoolOrDefault(string? value, bool defaultValue) =>
        string.IsNullOrWhiteSpace(value) ? defaultValue : bool.TryParse(value.Trim(), out var r) ? r : defaultValue;
}
