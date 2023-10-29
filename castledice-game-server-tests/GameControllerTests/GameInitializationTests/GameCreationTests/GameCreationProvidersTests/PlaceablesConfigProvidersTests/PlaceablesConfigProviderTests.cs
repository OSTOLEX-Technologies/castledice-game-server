using castledice_game_logic.GameObjects.Configs;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlaceablesConfigProviders;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.PlaceablesConfigProvidersTests;

public class PlaceablesConfigProviderTests
{
    [Fact]
    public void GetPlaceablesConfig_ShouldReturnPlaceablesConfigWithKnightConfig_FromGivenKnightConfigProvider()
    {
        var knightConfig = new KnightConfig(1,2);
        var knightConfigProvider = new Mock<IKnightConfigProvider>();
        knightConfigProvider.Setup(x => x.GetKnightConfig()).Returns(knightConfig);
        var provider = new PlaceablesConfigProvider(knightConfigProvider.Object);
        
        var placeablesConfig = provider.GetPlaceablesConfig();
        
        Assert.Same(knightConfig, placeablesConfig.KnightConfig);
    }
}