using castledice_game_logic.BoardGeneration.ContentGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders;

public interface ITreesSpawnerProvider
{
    RandomTreesSpawner GetTreesSpawner();
}