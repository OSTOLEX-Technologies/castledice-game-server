using castledice_game_logic;
using castledice_game_logic.BoardGeneration.ContentGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;

public interface ICastlesSpawnerCreator
{
    CastlesSpawner GetCastlesSpawner(List<Player> players);
}