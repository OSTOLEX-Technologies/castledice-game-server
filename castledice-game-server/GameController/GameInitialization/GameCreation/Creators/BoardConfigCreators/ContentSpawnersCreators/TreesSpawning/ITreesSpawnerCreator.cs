using castledice_game_logic.BoardGeneration.ContentGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;

public interface ITreesSpawnerCreator
{
    RandomTreesSpawner GetTreesSpawner();
}