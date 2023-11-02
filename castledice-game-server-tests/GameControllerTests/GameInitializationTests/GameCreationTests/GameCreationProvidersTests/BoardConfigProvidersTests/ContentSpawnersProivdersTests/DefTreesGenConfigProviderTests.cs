using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests.ContentSpawnersProivdersTests;

//This is a shortened class name. Full class name: DefaultTreesGenerationConfigProviderTests
public class DefTreesGenConfigProviderTests
{
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(4, 5, 6)]
    [InlineData(7, 8, 9)]
    public void GetTreesGenerationConfig_ShouldReturnConfig_WithValuesFromConstructor(int minTreesCount,
        int maxTreesCount, int minDistanceBetweenTrees)
    {
        var configProvider = new DefaultTreesGenerationConfigProvider(minTreesCount, maxTreesCount, minDistanceBetweenTrees);
        
        var config = configProvider.GetTreesGenerationConfig();
        
        Assert.Equal(minTreesCount, config.MinTreesCount);
        Assert.Equal(maxTreesCount, config.MaxTreesCount);
        Assert.Equal(minDistanceBetweenTrees, config.MinDistanceBetweenTrees);
    }
}