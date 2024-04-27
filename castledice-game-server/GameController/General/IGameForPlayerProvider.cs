using castledice_game_logic;

namespace castledice_game_server.GameController.General;

public interface IGameForPlayerProvider
{
    public Game GetGame(int playerId);
    public bool PlayerIsInGame(int playerId);
}