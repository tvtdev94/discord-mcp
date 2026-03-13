# Changelog

## [1.0.0] - 2026-03-13

### Added
- 70+ MCP tools covering: messages, channels, roles, moderation, categories, threads, webhooks, events, emojis, automod, members, invites, server, users, contacts
- Operator attribution system (`DISCORD_OPERATOR_ID`) — smart message prefix + JSONL audit logging
- Prompt injection protection — sanitized `[DISCORD_CONTENT]` delimiters on all message output
- Toolset filtering (`DISCORD_DISABLED_TOOLSETS`) — disable tool groups by name
- Contact store — JSON-backed name→userId mapping with tag support
- MCP server instructions for LLM guidance
- `.mcp/server.json` for MCP Registry compatibility
- Docker support with multi-stage alpine build
- GitHub Actions CI/CD — build, lint, release (NuGet + GitHub Releases)
- NuGet global tool distribution (`dotnet tool install -g`)

### Security
- SSRF protection on emoji upload (HTTPS-only, 256KB cap)
- SSRF protection on webhook URL (discord.com host validation)
- Webhook token not exposed in list output
- Bot can only edit own messages
- Permission name validation (no silent failures)
- Timeout capped at 28 days (Discord limit)
- Message count clamped 1-100
- Atomic file writes for contact store
