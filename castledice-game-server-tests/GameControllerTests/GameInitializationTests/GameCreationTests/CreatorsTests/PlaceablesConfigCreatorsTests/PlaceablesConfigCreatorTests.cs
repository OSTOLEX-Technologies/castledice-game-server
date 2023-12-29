using castledice_game_logic.GameObjects.Configs;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlaceablesConfigCreators;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.PlaceablesConfigCreatorsTests;

public class PlaceablesConfigCreatorTests
{
    [Fact]
    public void GetPlaceablesConfig_ShouldReturnPlaceablesConfigWithKnightConfig_FromGivenKnightConfigCreator()
    {
        var knightConfig = new KnightConfig(1,2);
        var knightConfigCreator = new Mock<IKnightConfigCreator>();
        knightConfigCreator.Setup(x => x.GetKnightConfig()).Returns(knightConfig);
        var creator = new PlaceablesConfigCreator(knightConfigCreator.Object);
        
        var placeablesConfig = creator.GetPlaceablesConfig();
        
        Assert.Same(knightConfig, placeablesConfig.KnightConfig);
    }
}