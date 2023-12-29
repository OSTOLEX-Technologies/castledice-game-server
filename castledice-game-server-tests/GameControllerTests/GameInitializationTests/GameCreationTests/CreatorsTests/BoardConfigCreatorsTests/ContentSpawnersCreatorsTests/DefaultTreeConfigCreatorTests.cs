using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.ContentSpawnersCreatorsTests;

public class DefaultTreeConfigCreatorTests
{
    [Theory]
    [InlineData(1, true)]
    [InlineData(10, false)]
    public void GetTreeConfig_ShouldReturnTreeConfig_WithValuesFromConstructor(int removeCost, bool canBeRemoved)
    {
        var configCreator = new DefaultTreeConfigCreator(removeCost, canBeRemoved);
        
        var config = configCreator.GetTreeConfig();
        
        Assert.Equal(removeCost, config.RemoveCost);
        Assert.Equal(canBeRemoved, config.CanBeRemoved);
    }
}