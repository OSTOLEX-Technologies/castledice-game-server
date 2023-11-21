using castledice_game_logic.BoardGeneration.ContentGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;

public class TreesSpawnerProvider : ITreesSpawnerProvider
{
    private readonly ITreesGenerationConfigProvider _treesGenerationConfigProvider;
    private readonly ITreesFactoryProvider _treesFactoryProvider;

    public TreesSpawnerProvider(ITreesGenerationConfigProvider treesGenerationConfigProvider, ITreesFactoryProvider treesFactoryProvider)
    {
        _treesGenerationConfigProvider = treesGenerationConfigProvider;
        _treesFactoryProvider = treesFactoryProvider;
    }

    public RandomTreesSpawner GetTreesSpawner()
    {
        var config = _treesGenerationConfigProvider.GetTreesGenerationConfig();
        var treesFactory = _treesFactoryProvider.GetTreesFactory();
        var treesSpawner = new RandomTreesSpawner(config.MinTreesCount, config.MaxTreesCount, config.MinDistanceBetweenTrees, treesFactory);
        return treesSpawner;
    }
}