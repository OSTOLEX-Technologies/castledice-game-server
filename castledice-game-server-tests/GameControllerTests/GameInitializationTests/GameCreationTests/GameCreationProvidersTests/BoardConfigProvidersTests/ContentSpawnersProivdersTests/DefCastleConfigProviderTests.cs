using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests.ContentSpawnersProivdersTests;

//This is a shortened class name. Full name: DefaultCastleConfigProviderTests.
public class DefCastleConfigProviderTests
{
    [Theory]
    [InlineData(3, 1, 1)]
    [InlineData(4, 2, 2)]
    [InlineData(3, 3, 3)]
    public void GetCastleConfig_ShouldReturnConfig_WithValuesFromConstructor(int maxDurability, int maxFreeDurability,
        int captureHitCost)
    {
        var configProvider = new DefaultCastleConfigProvider(maxDurability, maxFreeDurability, captureHitCost);

        var config = configProvider.GetCastleConfig();
        
        Assert.Equal(maxDurability, config.MaxDurability);
        Assert.Equal(maxFreeDurability, config.MaxFreeDurability);
        Assert.Equal(captureHitCost, config.CaptureHitCost);
    }
}