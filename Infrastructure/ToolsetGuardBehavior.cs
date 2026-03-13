namespace DiscordMcp.Infrastructure;

/// <summary>
/// MediatR pipeline behavior that blocks requests whose namespace belongs to a
/// toolset disabled via the DISCORD_DISABLED_TOOLSETS environment variable.
///
/// Namespace convention: DiscordMcp.Features.{ToolsetName}.* maps to toolset "{toolsetname}".
/// Example: DiscordMcp.Features.Moderation.BanUser → toolset "moderation"
/// </summary>
public sealed class ToolsetGuardBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    // Segment index of the toolset name in "DiscordMcp.Features.{Name}.*"
    private const string FeaturesSegment = "Features";

    public Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var toolset = ResolveToolset(typeof(TRequest));
        if (toolset is not null && ToolsetFilter.IsToolsetDisabled(toolset))
            throw new InvalidOperationException(
                $"Toolset '{toolset}' is disabled via DISCORD_DISABLED_TOOLSETS.");

        return next();
    }

    private static string? ResolveToolset(Type requestType)
    {
        var ns = requestType.Namespace;
        if (string.IsNullOrEmpty(ns))
            return null;

        var parts = ns.Split('.');
        for (var i = 0; i < parts.Length - 1; i++)
        {
            if (parts[i] == FeaturesSegment)
                return parts[i + 1].ToLowerInvariant();
        }

        return null;
    }
}
