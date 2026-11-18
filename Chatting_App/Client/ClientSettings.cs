using System.Text.Json;
using ChatShared;

namespace Client;

// I remember the last name, host and port the user typed so they do not have to retype them every time they open the app.
// The values live in a small JSON file under %LocalAppData%\ChatApp.
internal sealed record ClientSettings(string UserName, string Host, int Port)
{
    private static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "ChatApp",
        "client-settings.json");

    public static ClientSettings Default => new(Environment.UserName, ChatDefaults.ClientHost, ChatDefaults.Port);

    // If the file is missing or broken I just fall back to the defaults, because losing saved settings is never worth a crash.
    public static ClientSettings Load()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                return JsonSerializer.Deserialize<ClientSettings>(File.ReadAllText(FilePath)) ?? Default;
            }
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
        }

        return Default;
    }

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(this));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // Saving is only a convenience, so if the folder is read only I simply carry on.
        }
    }
}
