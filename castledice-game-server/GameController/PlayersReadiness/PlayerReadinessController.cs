using castledice_game_logic;
using castledice_game_server.Auth;
using castledice_game_server.GameController.Moves;
using castledice_game_server.Logging;

namespace castledice_game_server.GameController.PlayersReadiness;

public class PlayerReadinessController : IPlayersReadinessController
{
    private readonly IIdRetriever _idRetriever;
    private readonly IGameForPlayerProvider _gameForPlayerProvider;
    private readonly IPlayersReadinessTracker _playersReadinessTracker;
    private readonly IGamePlayersReadinessNotifier _gamePlayersReadinessNotifier;
    private readonly ILogger _logger;

    public PlayerReadinessController(IIdRetriever idRetriever, IGameForPlayerProvider gameForPlayerProvider, IPlayersReadinessTracker playersReadinessTracker, IGamePlayersReadinessNotifier gamePlayersReadinessNotifier, ILogger logger)
    {
        _idRetriever = idRetriever;
        _gameForPlayerProvider = gameForPlayerProvider;
        _playersReadinessTracker = playersReadinessTracker;
        _gamePlayersReadinessNotifier = gamePlayersReadinessNotifier;
        _logger = logger;
    }

    public async Task SetPlayerReadyAsync(string token)
    {
        try
        {
            var playerId = await _idRetriever.RetrievePlayerIdAsync(token);
            _playersReadinessTracker.SetPlayerReadiness(playerId, true);
            var game = _gameForPlayerProvider.GetGame(playerId);
            if (PlayersAreReadyInGame(game))
            {
                _gamePlayersReadinessNotifier.NotifyPlayersAreReady(game);
            }
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
        }
    }
    
    private bool PlayersAreReadyInGame(Game game)
    {
        var playersIds = game.GetAllPlayersIds();
        foreach (var playerId in playersIds)
        {
            if (!_playersReadinessTracker.PlayerIsReady(playerId))
            {
                return false;
            }
        }

        return true;
    }
}