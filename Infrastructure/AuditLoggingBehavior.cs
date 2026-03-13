using System.Diagnostics;

namespace DiscordMcp.Infrastructure;

/// <summary>
/// MediatR pipeline behavior that automatically writes an audit log entry
/// for every command/query handled, measuring wall-clock duration.
/// Skipped silently when OperatorContext is not configured.
/// </summary>
public sealed class AuditLoggingBehavior<TRequest, TResponse>(
    OperatorContext operatorContext,
    AuditLogger auditLogger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!operatorContext.IsConfigured)
            return await next();

        var sw = Stopwatch.StartNew();
        try
        {
            return await next();
        }
        finally
        {
            sw.Stop();
            auditLogger.Write(
                operatorId: operatorContext.Id,
                operatorName: operatorContext.Username,
                action: typeof(TRequest).Name,
                parameters: request,
                durationMs: sw.ElapsedMilliseconds);
        }
    }
}
