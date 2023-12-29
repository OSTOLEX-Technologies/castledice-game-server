using castledice_game_logic.GameObjects.Factories;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;

public interface ITreesFactoryCreator
{
    ITreesFactory GetTreesFactory();
}