using castledice_game_logic;

namespace castledice_game_server.GameController.Moves;

public interface IGameForPlayerProvider
{
    Game GetGame(int playerId);
}