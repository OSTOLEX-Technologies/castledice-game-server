using castledice_game_logic;

namespace castledice_game_server.GameController.PlayersReadiness;

public class GamePlayersReadinessNotifier : IGamePlayersReadinessNotifier
{
    public event EventHandler<Game>? PlayersAreReady;
    
    public void NotifyPlayersAreReady(Game game)
    {
        PlayersAreReady?.Invoke(this, game);
    }
}