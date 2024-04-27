using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;

namespace castledice_game_server.NetworkManager.PlayerDisconnection;

public class PlayerDisconnecter : IPlayerDisconnecter
{
    private readonly IClientDisconnecter _clientDisconnecter;
    private readonly IPlayerToClientIdsMap _idsMap;

    public PlayerDisconnecter(IClientDisconnecter clientDisconnecter,  IPlayerToClientIdsMap idsMap)
    {
        _clientDisconnecter = clientDisconnecter;
        _idsMap = idsMap;
    }

    public void DisconnectPlayerWithId(int playerId)
    {
        var clientId = _idsMap.GetClientByPlayer(playerId);
        _clientDisconnecter.DisconnectClient(clientId);
    }
}