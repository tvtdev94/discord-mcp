# Code Review: Features Group 2

**Scope:** 9 feature areas (Invites, Members, Messages, Moderation, Roles, Server, Threads, Users, Webhooks) - 40 handler files
**Focus:** Bugs, missing validation, security, null risks, incorrect Discord API usage

---

## Issues Found

### Critical

- **[CRITICAL]** `CreateWebhookHandler.cs:16` / `ListWebhooksHandler.cs:19` - **Webhook token leakage.** Webhook URLs (containing secret tokens) are returned as plaintext in MCP tool responses. Anyone with access to MCP tool output can use these URLs to impersonate the webhook and send arbitrary messages. Webhook tokens should be treated as secrets. Consider returning only the webhook ID and storing tokens securely, or at minimum warning the caller.

- **[CRITICAL]** `SendWebhookMessageHandler.cs:14` - **SSRF via webhook URL.** `DiscordWebhookClient(request.WebhookUrl)` accepts any user-supplied URL without validation that it's actually a `discord.com/api/webhooks/` URL. A malicious caller could supply an arbitrary URL to probe internal network endpoints. Must validate URL format before creating the client.

- **[CRITICAL]** `EditMessageHandler.cs:24` / `EditPrivateMessageHandler.cs:25` - **Bot can only edit its own messages.** No check that `msg.Author.Id == client.CurrentUser.Id`. Calling `ModifyAsync` on another user's message will throw a Discord API 403 error with a cryptic exception. Should validate ownership first and return a clear error.

### Important

- **[IMPORTANT]** `TimeoutMemberHandler.cs:11` - **No upper bound on timeout duration.** Discord limits timeouts to 28 days (40320 minutes). Only checks `> 0` but does not cap at 40320. Discord API will reject values exceeding this, producing an unhelpful error. Add: `if (request.DurationMinutes > 40320)`.

- **[IMPORTANT]** `ReadMessagesHandler.cs:12` - **No upper bound on message count.** `SafeParser.ParseIntOrDefault(request.Count, 100)` accepts any int. Discord `GetMessagesAsync` will internally handle it, but very large values cause excessive memory use and slow responses. Should clamp: `Math.Clamp(limit, 1, 100)`.

- **[IMPORTANT]** `ReadPrivateMessagesHandler.cs:12` - **Same unbounded count issue** as ReadMessages above.

- **[IMPORTANT]** `BanMemberHandler.cs:19` - **Deprecated API usage.** `guild.AddBanAsync(userId, deleteMessageDays, reason, options)` -- the `pruneDays` (int) overload is deprecated in Discord.Net 3.x. The recommended approach is `AddBanAsync(userId, deleteMsgSeconds, options)` where `deleteMessageSeconds` is used instead. This may compile with warnings but could break in future versions.

- **[IMPORTANT]** `AddMultipleReactionsHandler.cs:21-22` - **No rate limit handling.** Discord imposes strict rate limits on `AddReactionAsync` (1/0.25s per reaction). Sequential `await` in a tight loop with no delay will trigger rate limits on messages with many reactions. Consider adding a small delay or batching.

- **[IMPORTANT]** `CreateRoleHandler.cs:21` - **Negative color cast bug.** `(uint)colorVal` where `colorVal` is `int` -- if a negative int is parsed (e.g., user passes "-1"), this produces an unchecked uint conversion. Should validate `colorVal >= 0` or parse as uint directly.

- **[IMPORTANT]** `EditRoleHandler.cs:26-29` - **Same negative color cast issue** as CreateRoleHandler.

- **[IMPORTANT]** `ListWebhooksHandler.cs:15` - **Throws exception for empty list instead of returning empty result.** `throw new ArgumentException("No webhooks found.")` is inconsistent with every other List* handler (ListInvites, ListRoles, ListMembers, etc.) which return a friendly "none found" message. This will surface as an error to MCP callers.

- **[IMPORTANT]** `EditServerHandler.cs:8-9` - **Redundant null check.** Line 8 throws if `Name is null`, then line 16 checks `Name is not null` again before setting. The first check makes the second dead code. More importantly, the handler rejects requests where Name is null, but the command accepts `Name?` and `Description?` -- so if a caller wants to update only the description, they can't (and the handler acknowledges description isn't modifiable). The error message is misleading: "name must be provided (description is not modifiable via the bot API)" should be clearer about what's actually supported.

- **[IMPORTANT]** `CreateInviteHandler.cs:11` - **No guild context validation.** Creates invite on any channel by ID. A caller can create invites for channels in guilds the bot wasn't intended to operate on (if the bot is in multiple guilds). Other invite operations (Delete, List) use GuildResolver for scoping, but Create does not.

### Minor

- **[MINOR]** `GetAuditLogHandler.cs:21` - **Null user ID in string interpolation.** `e.User?.Id` will print empty string when User is null, producing output like `(ID: )`. Should use `e.User?.Id.ToString() ?? "unknown"`.

- **[MINOR]** `ListActiveThreadsHandler.cs:21-22` - **Swallows all exceptions silently.** Bare `catch` with no logging hides permission errors, network failures, and bugs. Should catch specific exception types (e.g., `HttpException` for 403) or at minimum log.

- **[MINOR]** `DeletePrivateMessageHandler.cs:24-29`, `EditPrivateMessageHandler.cs:29-34`, `ReadPrivateMessagesHandler.cs:22-27`, `SendPrivateMessageHandler.cs:22-27` - **Duplicated `GetUserByIdAsync` method** across 4 handler files. Should be extracted to a shared utility.

- **[MINOR]** `GetUserIdByNameHandler.cs:27` - **Discriminator matching is legacy.** Discord migrated away from discriminators to unique usernames. Discriminator is now "0" for migrated users. The `#discriminator` parsing logic will produce unexpected results for modern Discord users where `#0` is meaningless.

- **[MINOR]** `PruneMembersHandler.cs:13` - **Prune is a destructive, irreversible operation** with no confirmation mechanism. A value of `Days=1` prunes everyone inactive for just 1 day. Consider at minimum logging the prune action.

- **[MINOR]** `BulkDeleteMessagesHandler.cs:21` - **Count mismatch not reported.** If user requests 50 deletions but only 30 are eligible (< 14 days old), the response says "Bulk deleted 30" but doesn't mention 20 were skipped. Could confuse callers.

---

## Summary

40 handlers reviewed. 3 critical (webhook token exposure, SSRF, ownership check), 9 important, 6 minor issues found. The codebase is well-structured with consistent patterns, but security around webhooks and input bounds checking need immediate attention.
