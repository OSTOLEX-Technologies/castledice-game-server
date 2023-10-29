namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;

public class TreesGenerationConfig
{
    public int MaxTreesCount { get; }
    public int MinTreesCount { get; }
    public int MinDistanceBetweenTrees { get; }

    public TreesGenerationConfig(int maxTreesCount, int minTreesCount, int minDistanceBetweenTrees)
    {
        MaxTreesCount = maxTreesCount;
        MinTreesCount = minTreesCount;
        MinDistanceBetweenTrees = minDistanceBetweenTrees;
    }
}