using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.ContentSpawnersCreatorsTests;

public class TreesGenerationConfigTests
{
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(10, 20, 30)]
    [InlineData(15, 12, 13)]
    public void Properties_ShouldReturnValues_GivenInConstructor(int minTreesAmount, int maxTreesAmount,
        int minDistanceBetweenTrees)
    {
        var config = new TreesGenerationConfig(maxTreesAmount, minTreesAmount, minDistanceBetweenTrees);
        
        Assert.Equal(minTreesAmount, config.MinTreesCount);
        Assert.Equal(maxTreesAmount, config.MaxTreesCount);
        Assert.Equal(minDistanceBetweenTrees, config.MinDistanceBetweenTrees);
    }
}