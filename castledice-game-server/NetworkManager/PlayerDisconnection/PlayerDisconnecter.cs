﻿using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;

namespace castledice_game_server.NetworkManager.PlayerDisconnection;

public class PlayerDisconnecter : IPlayerDisconnecter
{
    private readonly IClientDisconnecter _clientDisconnecter;
    private readonly IPlayerClientIdRemover _playerClientIdRemover;
    private readonly IPlayerClientIdProvider _playerClientIdProvider;

    public PlayerDisconnecter(IClientDisconnecter clientDisconnecter, IPlayerClientIdRemover playerClientIdRemover, IPlayerClientIdProvider playerClientIdProvider)
    {
        _clientDisconnecter = clientDisconnecter;
        _playerClientIdRemover = playerClientIdRemover;
        _playerClientIdProvider = playerClientIdProvider;
    }

    public void DisconnectPlayerWithId(int playerId)
    {
        var clientId = _playerClientIdProvider.GetClientIdForPlayer(playerId);
        _clientDisconnecter.DisconnectClient(clientId);
        _playerClientIdRemover.RemoveClientIdForPlayer(playerId);
    }
}