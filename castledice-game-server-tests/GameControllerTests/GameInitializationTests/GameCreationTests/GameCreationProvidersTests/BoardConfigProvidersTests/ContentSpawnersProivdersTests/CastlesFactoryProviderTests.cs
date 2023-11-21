using castledice_game_logic.GameObjects.Configs;
using castledice_game_logic.GameObjects.Factories.Castles;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests.ContentSpawnersProivdersTests;

public class CastlesFactoryProviderTests
{
    [Fact]
    public void GetCastlesFactory_ShouldReturnFactory_WithConfigFromGivenConfigProvider()
    {
        var expectedConfig = new CastleConfig(3, 3, 3);
        var configProvider = new Mock<ICastleConfigProvider>();
        configProvider.Setup(provider => provider.GetCastleConfig()).Returns(expectedConfig);
        var provider = new CastlesFactoryProvider(configProvider.Object);
        
        var factory = provider.GetCastlesFactory();
        var castlesFactory = factory as CastlesFactory;
        
        Assert.Same(expectedConfig, castlesFactory.Config);
    }
}