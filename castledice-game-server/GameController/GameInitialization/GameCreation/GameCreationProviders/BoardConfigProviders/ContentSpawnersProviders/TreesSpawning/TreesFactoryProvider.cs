using castledice_game_logic.GameObjects.Factories;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;

public class TreesFactoryProvider : ITreesFactoryProvider
{
    private readonly ITreeConfigProvider _treeConfigProvider;
    
    public TreesFactoryProvider(ITreeConfigProvider treeConfigProvider)
    {
        _treeConfigProvider = treeConfigProvider;
    }
    
    public ITreesFactory GetTreesFactory()
    {
        throw new NotImplementedException();
    }
}