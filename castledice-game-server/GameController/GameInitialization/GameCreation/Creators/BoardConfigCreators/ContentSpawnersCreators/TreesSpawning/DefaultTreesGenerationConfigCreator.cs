namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;

public class DefaultTreesGenerationConfigCreator : ITreesGenerationConfigCreator
{
    private readonly int _minTreesCount;
    private readonly int _maxTreesCount;
    private readonly int _minDistanceBetweenTrees;

    public DefaultTreesGenerationConfigCreator(int minTreesCount, int maxTreesCount, int minDistanceBetweenTrees)
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