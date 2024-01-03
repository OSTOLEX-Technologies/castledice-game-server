using castledice_game_logic;
using castledice_game_server.GameController.PlayersReadiness;
using castledice_game_server.Logging;

namespace castledice_game_server.GameController.Timers;

public class TimersController
{
    private readonly IGamePlayersReadinessNotifier _readinessNotifier;
    private readonly IGamesCollection _gamesCollection;
    private readonly ITimerSwitchSender _timerSwitchSender;
    private readonly ILogger _logger;

    public TimersController(IGamePlayersReadinessNotifier readinessNotifier, IGamesCollection gamesCollection, ITimerSwitchSender timerSwitchSender, ILogger logger)
    {
        _readinessNotifier = readinessNotifier;
        _gamesCollection = gamesCollection;
        _timerSwitchSender = timerSwitchSender;
        _logger = logger;
        _readinessNotifier.PlayersAreReady += OnAllPlayersReady;
        _gamesCollection.GameAdded += OnGameAdded;
        _gamesCollection.GameRemoved += OnGameRemoved;
    }

    public virtual void SwitchTimersForPlayers(Game game)
    {
        
    }

    private void OnGameAdded(object? sender, Game game)
    {
        game.TurnSwitched += OnTurnSwitched;
    }
    
    private void OnGameRemoved(object? sender, Game game)
    {
        game.TurnSwitched -= OnTurnSwitched;
    }
    
    private void OnTurnSwitched(object? sender, Game game)
    {
        SwitchTimersForPlayers(game);
    }
    
    private void OnAllPlayersReady(object? sender, Game game)
    {
        SwitchTimersForPlayers(game);
    }
}