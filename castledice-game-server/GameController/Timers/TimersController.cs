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
        try
        {
            StopTimerForPreviousPlayer(game);
            SendPreviousPlayerTimeLeftToAllPlayers(game);
            StartTimerForCurrentPlayer(game);
            SendCurrentPlayerTimeLeftToAllPlayers(game);
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
        }
    }
    
    private void StopTimerForPreviousPlayer(Game game)
    {
        var previousPlayer = game.GetPreviousPlayer();
        previousPlayer.Timer.Stop();
    }
    
    private void SendPreviousPlayerTimeLeftToAllPlayers(Game game)
    {
        var previousPlayer = game.GetPreviousPlayer();
        var previousPlayerTimeLeft = previousPlayer.Timer.GetTimeLeft();
        foreach (var player in game.GetAllPlayers())
        {
            _timerSwitchSender.SendTimerSwitch(previousPlayer.Id, previousPlayerTimeLeft, player.Id, false);
        }
    }
    
    private void StartTimerForCurrentPlayer(Game game)
    {
        var currentPlayer = game.GetCurrentPlayer();
        currentPlayer.Timer.Start();
    }
    
    private void SendCurrentPlayerTimeLeftToAllPlayers(Game game)
    {
        var currentPlayer = game.GetCurrentPlayer();
        var currentPlayerTimeLeft = currentPlayer.Timer.GetTimeLeft();
        foreach (var player in game.GetAllPlayers())
        {
            _timerSwitchSender.SendTimerSwitch(currentPlayer.Id, currentPlayerTimeLeft, player.Id, true);
        }
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