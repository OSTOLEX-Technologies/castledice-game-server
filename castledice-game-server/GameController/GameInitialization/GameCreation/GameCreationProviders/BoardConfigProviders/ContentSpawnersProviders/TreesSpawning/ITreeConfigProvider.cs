using castledice_game_logic.GameObjects.Configs;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;

public interface ITreeConfigProvider
{
    TreeConfig GetTreeConfig();
}