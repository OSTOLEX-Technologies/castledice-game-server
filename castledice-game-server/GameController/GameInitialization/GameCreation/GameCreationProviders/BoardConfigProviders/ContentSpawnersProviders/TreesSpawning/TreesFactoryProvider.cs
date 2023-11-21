using castledice_game_logic.GameObjects.Factories;
using castledice_game_logic.GameObjects.Factories.Trees;

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
        var config = _treeConfigProvider.GetTreeConfig();
        return new TreesFactory(config);
    }
}