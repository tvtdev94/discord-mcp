# Refactor Report — GuildResolver + SafeParser consolidation

## Summary

Removed all duplicated `DefaultGuildId` fields and `ResolveGuildId()` helper methods from handler files in the six target feature directories. Replaced inline `ulong.Parse` / `int.Parse` calls with `SafeParser` equivalents.

## Files changed

### Guild resolver deduplication (DefaultGuildId + ResolveGuildId removed)

| File | Action |
|------|--------|
| `Features/Server/EditServer/EditServerHandler.cs` | Replaced with `GuildResolver.Resolve` |
| `Features/Server/GetServerInfo/GetServerInfoHandler.cs` | Replaced with `GuildResolver.Resolve` |
| `Features/Threads/ListActiveThreads/ListActiveThreadsHandler.cs` | Replaced with `GuildResolver.Resolve` |
| `Features/Users/GetUserIdByName/GetUserIdByNameHandler.cs` | Replaced with `GuildResolver.Resolve` |

### SafeParser — inline ulong/int.Parse replaced

| File | Replaced |
|------|---------|
| `Features/Threads/ArchiveThread/ArchiveThreadHandler.cs` | `ulong.Parse(threadId)` -> `SafeParser.ParseUlong` |
| `Features/Threads/UnarchiveThread/UnarchiveThreadHandler.cs` | `ulong.Parse(threadId)` -> `SafeParser.ParseUlong` |
| `Features/Threads/ListArchivedThreads/ListArchivedThreadsHandler.cs` | `ulong.Parse(channelId)` -> `SafeParser.ParseUlong` |
| `Features/Users/DeletePrivateMessage/DeletePrivateMessageHandler.cs` | `ulong.Parse(userId/messageId)` -> `SafeParser.ParseUlong` |
| `Features/Users/EditPrivateMessage/EditPrivateMessageHandler.cs` | `ulong.Parse(userId/messageId)` -> `SafeParser.ParseUlong` |
| `Features/Contacts/SetContact/SetContactHandler.cs` | Manual `ulong.TryParse` guard -> `SafeParser.ParseUlong` |

### Already clean (no changes needed)

- `Features/Roles/` — all 6 handlers already refactored
- `Features/Threads/CreateThread/CreateThreadHandler.cs` — already refactored
- `Features/Users/ReadPrivateMessages/ReadPrivateMessagesHandler.cs` — already refactored
- `Features/Users/SendPrivateMessage/SendPrivateMessageHandler.cs` — already refactored
- `Features/Webhooks/CreateWebhook/CreateWebhookHandler.cs` — already refactored
- `Features/Webhooks/DeleteWebhook/DeleteWebhookHandler.cs` — already refactored
- `Features/Webhooks/ListWebhooks/ListWebhooksHandler.cs` — already refactored
- `Features/Webhooks/SendWebhookMessage/SendWebhookMessageHandler.cs` — no IDs, no guild
- `Features/Contacts/GetContact|ListContacts|RemoveContact|TagContact` — already clean or no IDs

### Infrastructure/ContactStore.cs

Already has proper `Lock _lock` field with all mutating/reading methods wrapped in `lock (_lock)`. No changes required.

## Build result

C# compilation: 0 errors, 0 CS warnings.
MSB3027/MSB3021 file-lock errors in output: not a code issue — running `discord-mcp-csharp.exe` (PID 274960) held the output binary. Source compiles clean.

## Unresolved questions

None.
