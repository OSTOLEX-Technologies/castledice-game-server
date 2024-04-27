using castledice_game_logic;
using castledice_game_server.GameController.General;
using castledice_game_server.NetworkManager.PlayersTracking;

namespace castledice_game_server.GameController.PlayersDisconnectHandling;

public class PlayerDisconnectController
{
    private readonly IPlayerDisconnectedEventEmitter _eventEmitter;
    private readonly IPlayerToClientIdsMap _idsMap;
    private readonly IGamesCollection _gamesCollection;
    private readonly IGameForPlayerProvider _gameForPlayerProvider;
    private readonly IPlayerDisconnectedSender _disconnectedSender;

    public PlayerDisconnectController(IPlayerDisconnectedEventEmitter eventEmitter, IPlayerToClientIdsMap idsMap, IGamesCollection gamesCollection, IGameForPlayerProvider gameForPlayerProvider, IPlayerDisconnectedSender disconnectedSender)
    {
        _eventEmitter = eventEmitter;
        _idsMap = idsMap;
        _gameForPlayerProvider = gameForPlayerProvider;
        _disconnectedSender = disconnectedSender;
        _gamesCollection = gamesCollection;
        _eventEmitter.PlayerDisconnected += OnPlayerDisconnected;
    }

    private void OnPlayerDisconnected(int playerId)
    {
        if (_gameForPlayerProvider.PlayerIsInGame(playerId))
        {
            var game = _gameForPlayerProvider.GetGame(playerId);
            SendPlayerDisconnectedToOtherPlayersInGame(playerId, game);
            _gamesCollection.RemoveGame(game);
        }
        _idsMap.RemovePairByPlayer(playerId);
    }

    private void SendPlayerDisconnectedToOtherPlayersInGame(int disconnectedPlayerId, Game game)
    {
        foreach (var otherPlayerId in game.GetAllPlayersIds())
        {
            if (otherPlayerId != disconnectedPlayerId)
            {
                _disconnectedSender.SendPlayerDisconnectedMessage(disconnectedPlayerId, otherPlayerId);
            }
        }
    }
}