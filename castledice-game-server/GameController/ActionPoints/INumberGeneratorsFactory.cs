using castledice_game_logic.Math;

namespace castledice_game_server.GameController.ActionPoints;

public interface INumberGeneratorsFactory
{
    IRandomNumberGenerator GetGenerator();
}