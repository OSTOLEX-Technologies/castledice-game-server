using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.ContentSpawnersCreatorsTests;

//This is a shortened class name. Full name: DefaultCastleConfigCreatorTests.
public class DefCastleConfigCreatorTests
{
    [Theory]
    [InlineData(3, 1, 1)]
    [InlineData(4, 2, 2)]
    [InlineData(3, 3, 3)]
    public void GetCastleConfig_ShouldReturnConfig_WithValuesFromConstructor(int maxDurability, int maxFreeDurability,
        int captureHitCost)
    {
        var configCreator = new DefaultCastleConfigCreator(maxDurability, maxFreeDurability, captureHitCost);

        var config = configCreator.GetCastleConfig();
        
        Assert.Equal(maxDurability, config.MaxDurability);
        Assert.Equal(maxFreeDurability, config.MaxFreeDurability);
        Assert.Equal(captureHitCost, config.CaptureHitCost);
    }
}