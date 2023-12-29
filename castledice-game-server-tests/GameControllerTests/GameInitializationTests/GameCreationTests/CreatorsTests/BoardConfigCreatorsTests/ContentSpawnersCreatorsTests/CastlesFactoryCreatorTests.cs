using castledice_game_logic.GameObjects.Configs;
using castledice_game_logic.GameObjects.Factories.Castles;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.ContentSpawnersCreatorsTests;

public class CastlesFactoryCreatorTests
{
    [Fact]
    public void GetCastlesFactory_ShouldReturnFactory_WithConfigFromGivenConfigCreator()
    {
        var expectedConfig = new CastleConfig(3, 3, 3);
        var configCreator = new Mock<ICastleConfigCreator>();
        configCreator.Setup(creator => creator.GetCastleConfig()).Returns(expectedConfig);
        var creator = new CastlesFactoryCreator(configCreator.Object);
        
        var factory = creator.GetCastlesFactory();
        var castlesFactory = factory as CastlesFactory;
        
        Assert.Same(expectedConfig, castlesFactory.Config);
    }
}