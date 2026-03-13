# Code Review: Infrastructure & Program.cs

**Scope:** Program.cs, GlobalUsings.cs, .csproj, Infrastructure/*.cs (8 files)
**Focus:** Bugs, security, thread safety, error handling, resource management, null refs

## Issues Found

### Critical

None.

### Important

- **[Important]** `DiscordClientHostedService.cs:26-27` - Token read twice from env var. Already validated in Program.cs:9-14, but read again in the hosted service constructor. If env var is mutated between startup and DI resolution, they could diverge. More importantly, the token is stored as a plain `string` field for the lifetime of the process. Should inject the token via DI config (e.g., `IOptions<T>`) or at minimum pass it from Program.cs to avoid double-read and keep token handling in one place.

- **[Important]** `DiscordClientHostedService.cs:30-40` - No timeout on `_readySource.Task.WaitAsync(cancellationToken)`. If the Discord gateway never fires `Ready` (bad token, network issue, rate limit), the host hangs indefinitely until the OS-level cancellation token fires (default: no timeout on `HostOptions.StartupTimeout` unless configured). Add an explicit timeout, e.g., `WaitAsync(TimeSpan.FromSeconds(30), cancellationToken)`.

- **[Important]** `ContactStore.cs:64-68` - `Save()` uses `File.WriteAllText` which is not atomic. A crash or power loss mid-write corrupts the contacts file permanently. Write to a temp file then `File.Move` with overwrite for crash safety.

- **[Important]** `ContactStore.cs:57-61` - `Load()` has no error handling. Corrupted JSON (e.g., from the non-atomic write above) throws `JsonException` in the constructor, which crashes the entire host at startup with no recovery path. Wrap in try-catch, log warning, fall back to empty dict.

- **[Important]** `Program.cs:37` - `DiscordClientHostedService` registered via `AddSingleton<IHostedService, ...>()` but `DiscordSocketClient` is also singleton. If `StopAsync` is called and then `StartAsync` again (host restart scenarios), the event handlers are unsubscribed in `StopAsync:45-46` then re-subscribed in `StartAsync:32-33`, but `_readySource` is already completed and cannot be reset. Second startup would pass through the wait immediately without actually waiting for Ready. Minor in practice (hosts rarely restart without process restart), but a latent bug.

- **[Important]** `GuildResolver.cs:10-11` - `DefaultGuildId` is a `static readonly` field initialized once at class load time. If `DISCORD_GUILD_ID` is set after process start (unlikely but possible in container orchestration), the cached value is stale. Acceptable trade-off for perf, but document this behavior.

### Minor

- **[Minor]** `ContactStore.cs:13` - `_contacts` field is non-nullable but assigned in constructor via `Load()`. The field should be declared with `= null!;` or made `readonly` after assignment to make intent clear. Currently it's mutable but only ever assigned once.

- **[Minor]** `MessageFormatter.cs:14` - `m.Content` is interpolated directly into a markdown code block (triple backticks). If message content itself contains triple backticks, the formatting breaks. Escape or use a different delimiter.

- **[Minor]** `GlobalUsings.cs:2` - Comment says "237+ source files" which is a magic number that will become stale. Remove the count.

- **[Minor]** `Program.cs:12-13` - `Console.Error.WriteLine` + `Environment.Exit(1)` is abrupt. No chance for DI cleanup. Acceptable for pre-host-build validation, but `throw new InvalidOperationException()` would be more idiomatic and testable.

- **[Minor]** `SafeParser.cs` - `ParseIntOrDefault` silently swallows invalid input by returning `defaultValue`. Callers may not realize they got the default due to malformed input vs. genuinely absent input. Consider logging or distinguishing the two cases.

## Unresolved Questions

- Is there a graceful reconnect strategy if Discord disconnects mid-session, or does the bot just die? `DiscordSocketClient` handles reconnect internally, but the hosted service has no `Disconnected` event handler to log or act on it.
- Are there any rate-limit concerns with 70+ MCP tools all hitting the Discord API through a single `DiscordSocketClient`?
