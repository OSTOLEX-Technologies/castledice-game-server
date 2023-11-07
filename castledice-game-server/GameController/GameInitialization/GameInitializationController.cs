using castledice_game_logic.TurnsLogic;
using castledice_game_server.GameController.GameInitialization.GameCreation;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation;
using castledice_game_server.GameService;
using castledice_game_server.Logging;
using castledice_game_server.NetworkManager;

namespace castledice_game_server.GameController.GameInitialization;

public class GameInitializationController : IGameInitializationController
{
    private readonly IGameSavingService _gameSavingService;
    private readonly IGamesCollection _activeGamesCollection;
    private readonly IGameStartDataSender _gameStartDataSender;
    private readonly IGameCreator _gameCreator;
    private readonly IGameStartDataCreator _gameStartDataCreator;
    private readonly ILogger _logger;

    public GameInitializationController(IGameSavingService gameSavingService, IGamesCollection activeGamesCollection, IGameStartDataSender gameStartDataSender, IGameCreator gameCreator, IGameStartDataCreator gameStartDataCreator, ILogger logger)
    {
        _gameSavingService = gameSavingService;
        _activeGamesCollection = activeGamesCollection;
        _gameStartDataSender = gameStartDataSender;
        _gameCreator = gameCreator;
        _gameStartDataCreator = gameStartDataCreator;
        _logger = logger;
    }

    public async Task InitializeGameAsync(List<int> playersIds)
    {
        try
        {
            //TODO: add proper logic for adding turn switch conditions.
            var game = _gameCreator.CreateGame(playersIds);
            game.AddTurnSwitchCondition(new ActionPointsCondition(game.CurrentPlayerProvider));
            var gameStartData = _gameStartDataCreator.CreateGameStartData(game);
            var gameId = await _gameSavingService.SaveGameStartAsync(gameStartData);
            _activeGamesCollection.AddGame(gameId, game);
            _gameStartDataSender.SendGameStartData(gameStartData);
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
        }
    }
}