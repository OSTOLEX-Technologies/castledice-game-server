using castledice_game_logic;

namespace castledice_game_server.GameController.PlayersReadiness;

public interface IGamePlayersReadinessNotifier
{
    public event EventHandler<Game> PlayersAreReady; 
    void NotifyPlayersAreReady(Game game);
}