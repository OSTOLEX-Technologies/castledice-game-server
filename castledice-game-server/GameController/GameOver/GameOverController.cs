using castledice_game_logic;
using castledice_game_server.GameService;
using castledice_game_server.Logging;

namespace castledice_game_server.GameController.GameOver;

public class GameOverController
{
    private readonly IGamesCollection _gamesCollection;
    private readonly IGameSavingService _gameSavingService;
    private readonly IHistoryProvider _historyProvider;
    private readonly ILogger _logger;

    public GameOverController(IGamesCollection gamesCollection, IGameSavingService gameSavingService, IHistoryProvider historyProvider, ILogger logger)
    {
        _gamesCollection = gamesCollection;
        _gameSavingService = gameSavingService;
        _historyProvider = historyProvider;
        _logger = logger;
        _gamesCollection.GameAdded += OnGameAdded;
    }

    public virtual async Task SaveWin(Game game, Player winner)
    {
        await SaveGameResult(game, winner.Id);
    }

    public virtual async Task SaveDraw(Game game)
    {
        await SaveGameResult(game);
    }

    private async Task SaveGameResult(Game game, int? winnerId = null)
    {
        try
        {
            game.Win -= OnWin;
            game.Draw -= OnDraw;
            var gameId = _gamesCollection.GetGameId(game);
            _gamesCollection.RemoveGame(gameId);
            var history = _historyProvider.GetGameHistory(game);
            await _gameSavingService.SaveGameEndAsync(gameId, history, winnerId);
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
        }
    }
    
    private void OnGameAdded(object? sender, Game game)
    {
        game.Win += OnWin;
        game.Draw += OnDraw;
    }
    
    private async void OnWin(object? sender, (Game game, Player winner) data)
    {
        await SaveWin(data.game, data.winner);
    }

    private async void OnDraw(object? sender, Game game)
    {
        await SaveDraw(game);
    }
}