namespace castledice_game_server.NetworkManager;

/// <summary>
/// This class is used to map players ids to connected clients ids.
/// </summary>
public static class PlayersDictionary
{
    public static Dictionary<int, int> Dictionary { get; } = new();
}