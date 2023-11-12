using castledice_game_logic;
using castledice_game_server.Logging;

namespace castledice_game_server.GameController.ActionPoints;

public class ActionPointsController
{
    private readonly IGamesCollection _activeGames;
    private readonly INumberGeneratorsCollection _numberGenerators;
    private readonly IActionPointsSender _actionPointsSender;
    private readonly ILogger _logger;

    public ActionPointsController(IGamesCollection activeGames, INumberGeneratorsCollection numberGenerators,
        IActionPointsSender actionPointsSender, ILogger logger)
    {
        _activeGames = activeGames;
        _numberGenerators = numberGenerators;
        _actionPointsSender = actionPointsSender;
        _logger = logger;
        _activeGames.GameAdded += OnGameAdded;
        _activeGames.GameRemoved += OnGameRemoved;
    }

    public virtual void GiveActionPointsToCurrentPlayer(Game game)
    {
        try
        {
            var currentPlayer = game.GetCurrentPlayer();
            var generator = _numberGenerators.GetGeneratorForPlayer(currentPlayer.Id);
            var actionPoints = generator.GetNextRandom();
            game.GiveActionPointsToPlayer(currentPlayer.Id, actionPoints);
            var playersIds = game.GetAllPlayersIds();
            foreach (var playerId in playersIds)
            {
                _actionPointsSender.SendActionPoints(actionPoints, currentPlayer.Id, playerId);
            }
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
        }
    }

    private void OnGameAdded(object? sender, Game game)
    {
        try
        {
            var playersIds = game.GetAllPlayersIds();
            foreach (var playerId in playersIds)
            {
                _numberGenerators.AddGeneratorForPlayer(playerId);
            }
            game.TurnSwitched += OnTurnSwitched;
            GiveActionPointsToCurrentPlayer(game); //TODO: This should be fixed
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
        }
    }

    private void OnGameRemoved(object? sender, Game game)
    {
        var playersIds = game.GetAllPlayersIds();
        foreach (var playerId in playersIds)
        {
            _numberGenerators.RemoveGeneratorForPlayer(playerId);
        }
        game.TurnSwitched -= OnTurnSwitched;
    }

    private void OnTurnSwitched(object? sender, Game game)
    {
        GiveActionPointsToCurrentPlayer(game);
    }
}