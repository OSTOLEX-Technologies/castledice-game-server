using castledice_game_logic.GameObjects.Configs;
using castledice_game_logic.GameObjects.Factories.Trees;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests.ContentSpawnersProivdersTests;

public class TreesFactoryProviderTests
{
    [Fact]
    public void GetTreesFactory_ShouldReturnFactory_WithConfigFromGivenConfigProvider()
    {
        var expectedConfig = new TreeConfig(1, false);
        var configProvider = new Mock<ITreeConfigProvider>();
        configProvider.Setup(provider => provider.GetTreeConfig()).Returns(expectedConfig);
        var provider = new TreesFactoryProvider(configProvider.Object);
        
        var factory = provider.GetTreesFactory();
        var treesFactory = factory as TreesFactory;
        
        Assert.Same(expectedConfig, treesFactory.Config);
    }
}