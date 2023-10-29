using castledice_game_logic;
using castledice_game_logic.BoardGeneration.ContentGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;

public interface ICastlesSpawnerProvider
{
    CastlesSpawner GetCastlesSpawner(List<Player> players);
}