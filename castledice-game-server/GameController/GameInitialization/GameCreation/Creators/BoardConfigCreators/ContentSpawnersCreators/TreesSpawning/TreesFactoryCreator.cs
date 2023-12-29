using castledice_game_logic.GameObjects.Factories;
using castledice_game_logic.GameObjects.Factories.Trees;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;

public class TreesFactoryCreator : ITreesFactoryCreator
{
    private readonly ITreeConfigCreator _treeConfigCreator;
    
    public TreesFactoryCreator(ITreeConfigCreator treeConfigCreator)
    {
        _treeConfigCreator = treeConfigCreator;
    }
    
    public ITreesFactory GetTreesFactory()
    {
        var config = _treeConfigCreator.GetTreeConfig();
        return new TreesFactory(config);
    }
}