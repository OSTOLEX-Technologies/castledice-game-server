using castledice_game_logic.Math;

namespace castledice_game_server.GameController.ActionPoints;

public interface INumberGeneratorsCollection
{
    void AddGeneratorForPlayer(int playerId);
    IRandomNumberGenerator GetGeneratorForPlayer(int playerId);
    bool RemoveGeneratorForPlayer(int playerId);
}