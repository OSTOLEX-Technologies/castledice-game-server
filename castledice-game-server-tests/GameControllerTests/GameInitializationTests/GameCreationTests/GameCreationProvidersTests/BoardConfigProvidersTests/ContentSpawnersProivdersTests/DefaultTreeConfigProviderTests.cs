using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests.ContentSpawnersProivdersTests;

public class DefaultTreeConfigProviderTests
{
    [Theory]
    [InlineData(1, true)]
    [InlineData(10, false)]
    public void GetTreeConfig_ShouldReturnTreeConfig_WithValuesFromConstructor(int removeCost, bool canBeRemoved)
    {
        var configProvider = new DefaultTreeConfigProvider(removeCost, canBeRemoved);
        
        var config = configProvider.GetTreeConfig();
        
        Assert.Equal(removeCost, config.RemoveCost);
        Assert.Equal(canBeRemoved, config.CanBeRemoved);
    }
}