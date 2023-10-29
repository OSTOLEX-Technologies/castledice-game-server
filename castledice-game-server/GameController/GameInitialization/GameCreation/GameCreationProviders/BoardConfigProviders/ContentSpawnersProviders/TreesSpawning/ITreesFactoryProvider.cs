using castledice_game_logic.GameObjects.Factories;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;

public interface ITreesFactoryProvider
{
    ITreesFactory GetTreesFactory();
}