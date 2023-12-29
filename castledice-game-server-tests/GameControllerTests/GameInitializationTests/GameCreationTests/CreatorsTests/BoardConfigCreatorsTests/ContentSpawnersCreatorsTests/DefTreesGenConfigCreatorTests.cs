using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.ContentSpawnersCreatorsTests;

//This is a shortened class name. Full class name: DefaultTreesGenerationConfigCreatorTests
public class DefTreesGenConfigCreatorTests
{
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(4, 5, 6)]
    [InlineData(7, 8, 9)]
    public void GetTreesGenerationConfig_ShouldReturnConfig_WithValuesFromConstructor(int minTreesCount,
        int maxTreesCount, int minDistanceBetweenTrees)
    {
        var configCreator = new DefaultTreesGenerationConfigCreator(minTreesCount, maxTreesCount, minDistanceBetweenTrees);
        
        var config = configCreator.GetTreesGenerationConfig();
        
        Assert.Equal(minTreesCount, config.MinTreesCount);
        Assert.Equal(maxTreesCount, config.MaxTreesCount);
        Assert.Equal(minDistanceBetweenTrees, config.MinDistanceBetweenTrees);
    }
}