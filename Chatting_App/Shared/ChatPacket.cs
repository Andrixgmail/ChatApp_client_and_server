namespace ChatShared;

// These are all the kinds of packets I send between the client and the server.
public enum PacketType
{
    // The client sends this first, right after connecting, to tell me which name it wants.
    Join,

    // I send this back to a single client to confirm the name I actually gave it.
    Welcome,

    // I broadcast this when somebody leaves the chat.
    Leave,

    // A normal chat message.
    Message,

    // I broadcast the full list of online users whenever someone joins or leaves.
    UserList,

    // A general notice from the server, for example when it is shutting down.
    Notice
}

// This is the one object that travels over the wire. I serialize it to a single line of JSON,
// so every packet is just one line of text ending in a newline.
public sealed record ChatPacket(
    PacketType Type,
    string Sender,
    string Text,
    DateTimeOffset Timestamp,
    IReadOnlyList<string>? Users = null)
{
    public const int MaxNameLength = 24;
    public const int MaxMessageLength = 2000;

    // I use these small factory helpers so I never have to remember the constructor order.
    public static ChatPacket Join(string name) => new(PacketType.Join, name, string.Empty, DateTimeOffset.UtcNow);

    public static ChatPacket Welcome(string assignedName) => new(PacketType.Welcome, assignedName, string.Empty, DateTimeOffset.UtcNow);

    public static ChatPacket Leave(string name) => new(PacketType.Leave, name, string.Empty, DateTimeOffset.UtcNow);

    public static ChatPacket Message(string sender, string text) => new(PacketType.Message, sender, text, DateTimeOffset.UtcNow);

    public static ChatPacket UserList(IReadOnlyList<string> users) => new(PacketType.UserList, string.Empty, string.Empty, DateTimeOffset.UtcNow, users);

    public static ChatPacket Notice(string text) => new(PacketType.Notice, "Server", text, DateTimeOffset.UtcNow);

    // Here I tidy up a display name: no control characters, no extra spaces and never longer than the limit.
    public static string CleanName(string? raw)
    {
        string cleaned = StripControlCharacters(raw, keepNewLines: false).Trim();
        return cleaned.Length > MaxNameLength ? cleaned[..MaxNameLength].TrimEnd() : cleaned;
    }

    // Here I tidy up message text the same way, but I allow line breaks so people can send multi line messages.
    public static string CleanText(string? raw)
    {
        string cleaned = StripControlCharacters(raw, keepNewLines: true).Trim();
        return cleaned.Length > MaxMessageLength ? cleaned[..MaxMessageLength] : cleaned;
    }

    private static string StripControlCharacters(string? raw, bool keepNewLines)
    {
        if (string.IsNullOrEmpty(raw))
        {
            return string.Empty;
        }

        // I normalise Windows line endings first so I only have to think about '\n'.
        string text = raw.Replace("\r\n", "\n").Replace('\r', '\n');
        return new string(text.Where(c => !char.IsControl(c) || (keepNewLines && c == '\n')).ToArray());
    }
}

// The values both apps fall back to when the user has not typed anything yet.
public static class ChatDefaults
{
    public const int Port = 8910;
    public const string ClientHost = "127.0.0.1";
    public const string ServerBindAddress = "0.0.0.0";
}
