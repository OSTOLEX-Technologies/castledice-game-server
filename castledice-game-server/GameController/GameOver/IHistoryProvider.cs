using castledice_game_logic;

namespace castledice_game_server.GameController.GameOver;

public interface IHistoryProvider
{
    string GetGameHistory(Game game);
}