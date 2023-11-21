namespace castledice_game_server.NetworkManager.PlayersTracking;

public class PlayerToClientDictionary : IPlayerClientIdProvider, IPlayerClientIdRemover, IPlayerClientIdSaver
{
    private readonly Dictionary<int, ushort> _playerToClientIds = new();
    
    public ushort GetClientIdForPlayer(int playerId)
    {
        if (_playerToClientIds.TryGetValue(playerId, out var player)) return player;
        throw new InvalidOperationException("No client id found for given player id: " + playerId);
    }

    public bool PlayerHasClientId(int playerId)
    {
        return _playerToClientIds.ContainsKey(playerId);
    }

    public bool RemoveClientIdForPlayer(int playerId)
    {
        return _playerToClientIds.Remove(playerId);
    }

    public void SaveClientIdForPlayer(int playerId, ushort clientId)
    {
        if (_playerToClientIds.ContainsKey(playerId))
        {
            throw new InvalidOperationException("Client id for given player id already exists!. Player id: " + playerId + ", client id: " + clientId);
        }
        _playerToClientIds.Add(playerId, clientId);
    }
}