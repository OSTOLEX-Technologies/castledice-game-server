using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using Riptide;

namespace castledice_game_server.GameController.PlayersDisconnectHandling;

public class PlayerDisconnectedEventEmitter : IPlayerDisconnectedEventEmitter
{
    public event Action<int>? PlayerDisconnected;
    
    private readonly IServerWrapper _serverWrapper;
    private readonly IPlayerToClientIdsMap _idsMap;

    public PlayerDisconnectedEventEmitter(IServerWrapper serverWrapper, IPlayerToClientIdsMap idsMap)
    {
        _serverWrapper = serverWrapper;
        _idsMap = idsMap;
        _serverWrapper.ClientDisconnected += OnClientDisconnected;
    }

    private void OnClientDisconnected(object? sender, ServerDisconnectedEventArgs e)
    {
        var clientId = e.Client.Id;
        if (!_idsMap.ClientHasPlayer(clientId)) return;
        var playerId = _idsMap.GetPlayerByClient(clientId);
        PlayerDisconnected?.Invoke(playerId);
    }
}