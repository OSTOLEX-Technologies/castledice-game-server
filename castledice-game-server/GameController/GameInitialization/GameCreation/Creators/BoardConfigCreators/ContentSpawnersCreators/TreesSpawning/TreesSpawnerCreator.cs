using castledice_game_logic.BoardGeneration.ContentGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;

public class TreesSpawnerCreator : ITreesSpawnerCreator
{
    private readonly ITreesGenerationConfigCreator _treesGenerationConfigCreator;
    private readonly ITreesFactoryCreator _treesFactoryCreator;

    public TreesSpawnerCreator(ITreesGenerationConfigCreator treesGenerationConfigCreator, ITreesFactoryCreator treesFactoryCreator)
    {
        _treesGenerationConfigCreator = treesGenerationConfigCreator;
        _treesFactoryCreator = treesFactoryCreator;
    }

    public RandomTreesSpawner GetTreesSpawner()
    {
        var config = _treesGenerationConfigCreator.GetTreesGenerationConfig();
        var treesFactory = _treesFactoryCreator.GetTreesFactory();
        var treesSpawner = new RandomTreesSpawner(config.MinTreesCount, config.MaxTreesCount, config.MinDistanceBetweenTrees, treesFactory);
        return treesSpawner;
    }
}