using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlaceablesConfigProviders;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.PlaceablesConfigProvidersTests;

public class DefaultKnightConfigProviderTests
{
    [Theory]
    [InlineData(1, 2)]
    [InlineData(3, 4)]
    [InlineData(5, 6)]
    [InlineData(7, 8)]
    public void GetKnightConfig_ShouldReturnKnightConfigWithValues_GivenInConstructor(int placementCost, int health)
    {
        var provider = new DefaultKnightConfigProvider(placementCost, health);
        
        var knightConfig = provider.GetKnightConfig();
        
        Assert.Equal(placementCost, knightConfig.PlacementCost);
        Assert.Equal(health, knightConfig.Health);
    }
}