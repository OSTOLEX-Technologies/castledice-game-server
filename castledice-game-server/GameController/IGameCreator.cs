using castledice_game_logic;

namespace castledice_game_server.GameController;

public interface IGameCreator
{
    Game CreateGame(List<int> playersIds);
}