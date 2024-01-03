using castledice_game_logic;

namespace castledice_game_server.GameController.PlayersReadiness;

public interface IGamePlayersReadinessNotifier
{
    void NotifyPlayersAreReady(Game game);
    public event EventHandler<Game> PlayersAreReady; 
}