using castledice_game_logic.GameObjects.Configs;
using castledice_game_logic.GameObjects.Factories.Trees;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.ContentSpawnersCreatorsTests;

public class TreesFactoryCreatorTests
{
    [Fact]
    public void GetTreesFactory_ShouldReturnFactory_WithConfigFromGivenConfigCreator()
    {
        var expectedConfig = new TreeConfig(1, false);
        var configCreator = new Mock<ITreeConfigCreator>();
        configCreator.Setup(provider => provider.GetTreeConfig()).Returns(expectedConfig);
        var provider = new TreesFactoryCreator(configCreator.Object);
        
        var factory = provider.GetTreesFactory();
        var treesFactory = factory as TreesFactory;
        
        Assert.Same(expectedConfig, treesFactory.Config);
    }
}