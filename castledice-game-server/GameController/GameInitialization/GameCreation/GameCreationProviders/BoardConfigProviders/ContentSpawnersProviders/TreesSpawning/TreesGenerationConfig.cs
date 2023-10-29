namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;

public class TreesGenerationConfig
{
    public int MaxTreesAmount { get; }
    public int MinTreesAmount { get; }
    public int MinDistanceBetweenTrees { get; }

    public TreesGenerationConfig(int maxTreesAmount, int minTreesAmount, int minDistanceBetweenTrees)
    {
        MaxTreesAmount = maxTreesAmount;
        MinTreesAmount = minTreesAmount;
        MinDistanceBetweenTrees = minDistanceBetweenTrees;
    }
}