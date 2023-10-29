using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests.ContentSpawnersProivdersTests;

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
        
        Assert.Equal(minTreesAmount, config.MinTreesAmount);
        Assert.Equal(maxTreesAmount, config.MaxTreesAmount);
        Assert.Equal(minDistanceBetweenTrees, config.MinDistanceBetweenTrees);
    }
}