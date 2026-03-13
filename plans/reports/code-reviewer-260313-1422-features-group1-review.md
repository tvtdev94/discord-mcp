# Code Review: Features Group 1

**Scope:** AutoMod (4), Categories (4), Channels (11), Contacts (5), Emojis (3), Events (4) -- 31 handler files
**Date:** 2026-03-13

---

## Issues Found

### Critical

- **[Critical]** `Features/Emojis/CreateEmoji/CreateEmojiHandler.cs:18` -- **SSRF vulnerability.** `Http.GetStreamAsync(request.ImageUrl)` fetches any user-supplied URL with no validation. Attacker can probe internal network (`http://169.254.169.254/`, `http://localhost:xxxx/`, `file:///`). Must validate URL scheme (https only), block private/reserved IP ranges, and enforce a size limit on the downloaded content.

- **[Critical]** `Features/Emojis/CreateEmoji/CreateEmojiHandler.cs:21-23` -- **Unbounded memory allocation.** `stream.CopyToAsync(ms)` with no size cap. Malicious URL returning gigabytes of data will OOM the server. Add a max-size check (Discord emojis max 256KB).

- **[Critical]** `Features/Channels/SetChannelPermission/SetChannelPermissionHandler.cs:47-58` -- **Silent permission failures.** `ParsePermissions` silently skips unknown permission names. If a caller typos `"SendMessage"` as `"SndMessage"`, no permission is set and no error reported. The overwrite applies with `0ul` allow/deny -- effectively a no-op that returns success. Must warn or fail on unrecognized permission names.

### Important

- **[Important]** `Features/AutoMod/CreateAutoModRule/CreateAutoModRuleHandler.cs:17` -- `AutoModTriggerType.HarmfulLink` was deprecated by Discord and may not function. Discord.Net 3.x still has the enum but Discord API ignores it. The tool error message lists it as valid, misleading users.

- **[Important]** `Features/AutoMod/CreateAutoModRule/CreateAutoModRuleHandler.cs:12-18` -- Missing `keyword_preset` trigger type. Discord supports `KeywordPreset` for built-in wordlists (profanity, slurs, sexual content). Omitting it limits AutoMod functionality.

- **[Important]** `Features/AutoMod/CreateAutoModRule/CreateAutoModRuleHandler.cs:28` -- Hardcoded `BlockMessage` action. No way to set `SendAlertMessage`, `Timeout`, or other action types. Users cannot create rules that alert a channel or timeout users.

- **[Important]** `Features/Channels/EditChannel/EditChannelHandler.cs:13` -- Casts to `SocketTextChannel` only. Voice channels, forum channels, and stage channels cannot be edited (name change). Should cast to `SocketGuildChannel` and use `ModifyAsync` on the base type, or handle multiple channel types.

- **[Important]** `Features/Channels/DeleteChannelPermission/DeleteChannelPermissionHandler.cs:17` and `SetChannelPermissionHandler.cs:19` -- No guild-scoped resolution. Uses `client.GetChannel()` which works across all guilds the bot is in. Unlike other handlers that go through `GuildResolver.Resolve()`, these skip guild validation entirely. A user could manipulate channels in unintended guilds.

- **[Important]** `Features/Channels/MoveChannel/MoveChannelHandler.cs:11` -- Same as above: uses `client.GetChannel()` without guild scoping. No GuildId parameter in command record.

- **[Important]** `Features/Channels/EditChannel/EditChannelHandler.cs:8-9` -- Redundant validation. `ChannelId` is a non-nullable `string` in the record, and `SafeParser.ParseUlong` on line 13 already throws if empty. The explicit null check is dead code.

- **[Important]** `Features/Events/CreateScheduledEvent/CreateScheduledEventHandler.cs:31` -- Casts to `SocketVoiceChannel` only. Stage channels (`SocketStageChannel`) also support scheduled events but will fail the cast and throw a misleading "Voice channel not found" error.

- **[Important]** `Features/Events/CreateScheduledEvent/CreateScheduledEventHandler.cs:56` -- Hardcoded `location: "TBD"` for external events. Should accept a `Location` parameter from the command; Discord requires a meaningful location string for external events.

- **[Important]** `Features/Events/EditScheduledEvent/EditScheduledEventHandler.cs:33-39` -- No validation that all fields are null (no-op edit). If caller provides no fields to update, `ModifyAsync` runs with no mutations -- wasteful API call that returns success misleadingly.

### Minor

- **[Minor]** `Features/AutoMod/EditAutoModRule/EditAutoModRuleHandler.cs:23` -- Returns `rule.Name` after `ModifyAsync`, but the local `rule` object may not reflect the updated name (Discord.Net cache staleness). Should use `request.Name ?? rule.Name`.

- **[Minor]** `Features/Categories/ListChannelsInCategory/ListChannelsInCategoryHandler.cs:16-17` -- Throws `ArgumentException` when category has no channels. This is a valid state, not an error. Should return informational message instead.

- **[Minor]** `Features/Channels/ListChannels/ListChannelsHandler.cs:14` -- Throws when guild has no channels. Same issue -- valid state treated as error.

- **[Minor]** `Features/Channels/FindChannel/FindChannelHandler.cs:15-16` -- Throws "No channels found by guildId" when guild has zero channels. Misleading error: suggests guildId is wrong rather than guild being empty.

- **[Minor]** `Features/Contacts/TagContact/TagContactHandler.cs:21` -- No length validation on `request.Message`. Discord message limit is 2000 chars. Combined with the `<@userId>` prefix, could exceed limit and throw an unhelpful Discord API error.

- **[Minor]** `Features/Emojis/CreateEmoji/CreateEmojiHandler.cs:9` -- Static `HttpClient` without timeout configured. Default timeout is 100 seconds. For image fetching, should be much lower (e.g., 10s).

- **[Minor]** `Features/Events/EditScheduledEvent/EditScheduledEventHandler.cs:35-36` -- Uses `!string.IsNullOrWhiteSpace()` check for Name and Description, meaning caller cannot clear description to empty string. Inconsistent with `EditAutoModRuleHandler` which uses `is not null` pattern.

- **[Minor]** `Infrastructure/ContactStore.cs:60-61` -- `Load()` swallows corrupt JSON (returns empty dict). Silent data loss if file gets corrupted. Should log a warning.

---

## Summary

31 handlers reviewed. 3 critical (SSRF + OOM in CreateEmoji, silent permission failures in SetChannelPermission), 9 important (missing guild scoping on several channel handlers, limited channel type support, hardcoded values), 8 minor.

The most urgent fix is the SSRF in `CreateEmojiHandler` -- it is directly exploitable by any MCP client.
