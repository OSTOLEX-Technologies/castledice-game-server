namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;

public class DefaultTreesGenerationConfigProvider : ITreesGenerationConfigProvider
{
    private readonly int _minTreesCount;
    private readonly int _maxTreesCount;
    private readonly int _minDistanceBetweenTrees;

    public DefaultTreesGenerationConfigProvider(int minTreesCount, int maxTreesCount, int minDistanceBetweenTrees)
    {
        _minTreesCount = minTreesCount;
        _maxTreesCount = maxTreesCount;
        _minDistanceBetweenTrees = minDistanceBetweenTrees;
    }

    public TreesGenerationConfig GetTreesGenerationConfig()
    {
        return new TreesGenerationConfig(_maxTreesCount, _minTreesCount, _minDistanceBetweenTrees);
    }
}