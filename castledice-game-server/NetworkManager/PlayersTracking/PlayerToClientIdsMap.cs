namespace castledice_game_server.NetworkManager.PlayersTracking;

public class PlayerToClientIdsMap : IPlayerClientIdProvider, IPlayerClientIdRemover, IPlayerClientIdSaver, IPlayerToClientIdsMap
{
    private readonly Dictionary<int, ushort> _playerToClientIds = new();
    
    # region Deprecated
    
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
    
    # endregion

    public void SaveClientToPlayer(int playerId, ushort clientId)
    {
        if (_playerToClientIds.ContainsKey(playerId))
        {
            throw new InvalidOperationException("Client id for given player id already exists!. Player id: " + playerId + ", client id: " + clientId);
        }
        if (_playerToClientIds.Any(p => p.Value == clientId))
        {
            throw new InvalidOperationException("Player id for given client id already exists!. Player id: " + playerId + ", client id: " + clientId);
        }
        _playerToClientIds.Add(playerId, clientId);
    }

    public int GetPlayerByClient(ushort clientId)
    {
        if (!ClientHasPlayer(clientId))
        {
            throw new InvalidOperationException("No player id found for given client id: " + clientId);
        }
        return _playerToClientIds.First(p => p.Value == clientId).Key;
    }

    public ushort GetClientByPlayer(int playerId)
    {
        if (!PlayerHasClient(playerId))
        {
            throw new InvalidOperationException("No client id found for given player id: " + playerId);
        }
        return _playerToClientIds[playerId];
    }

    public bool PlayerHasClient(int playerId)
    {
        return _playerToClientIds.ContainsKey(playerId);
    }

    public bool ClientHasPlayer(ushort clientId)
    {
        return _playerToClientIds.Any(p => p.Value == clientId);
    }

    public bool RemovePairByClient(ushort clientId)
    {
        if (!ClientHasPlayer(clientId)) return false;
        return _playerToClientIds.Remove(_playerToClientIds.First(p => p.Value == clientId).Key);
    }

    public bool RemovePairByPlayer(int playerId)
    {
        if (!PlayerHasClient(playerId)) return false;
        return _playerToClientIds.Remove(playerId);
    }
}