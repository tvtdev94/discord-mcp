using System.Text.Json;

namespace DiscordMcp.Infrastructure;

/// <summary>
/// Thread-safe JSON-backed contact store for name → Discord user ID mappings.
/// Stored at: contacts.json in app directory (or DISCORD_CONTACTS_PATH env var).
/// </summary>
public sealed class ContactStore
{
    private readonly string _filePath;
    private readonly Lock _lock = new();
    private Dictionary<string, string> _contacts; // key: lowercase name, value: userId

    public ContactStore()
    {
        _filePath = Environment.GetEnvironmentVariable("DISCORD_CONTACTS_PATH")
            ?? Path.Combine(AppContext.BaseDirectory, "contacts.json");
        _contacts = Load();
    }

    public void Set(string name, string userId)
    {
        lock (_lock)
        {
            _contacts[name.ToLowerInvariant().Trim()] = userId.Trim();
            Save();
        }
    }

    public bool TryGet(string name, out string userId)
    {
        lock (_lock)
        {
            return _contacts.TryGetValue(name.ToLowerInvariant().Trim(), out userId!);
        }
    }

    public Dictionary<string, string> GetAll()
    {
        lock (_lock)
        {
            return new(_contacts);
        }
    }

    public bool Remove(string name)
    {
        lock (_lock)
        {
            var removed = _contacts.Remove(name.ToLowerInvariant().Trim());
            if (removed) Save();
            return removed;
        }
    }

    private Dictionary<string, string> Load()
    {
        if (!File.Exists(_filePath)) return new();
        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
        }
        catch (JsonException)
        {
            // Corrupt file — start fresh rather than crash the host
            return new();
        }
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_contacts,
            new JsonSerializerOptions { WriteIndented = true });
        // Write to temp file then rename for atomic save (prevents corruption on crash)
        var tempPath = _filePath + ".tmp";
        File.WriteAllText(tempPath, json);
        File.Move(tempPath, _filePath, overwrite: true);
    }
}
