using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlaceablesConfigCreators;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.PlaceablesConfigCreatorsTests;

public class DefaultKnightConfigCreatorTests
{
    [Theory]
    [InlineData(1, 2)]
    [InlineData(3, 4)]
    [InlineData(5, 6)]
    [InlineData(7, 8)]
    public void GetKnightConfig_ShouldReturnKnightConfigWithValues_GivenInConstructor(int placementCost, int health)
    {
        var creator = new DefaultKnightConfigCreator(placementCost, health);
        
        var knightConfig = creator.GetKnightConfig();
        
        Assert.Equal(placementCost, knightConfig.PlacementCost);
        Assert.Equal(health, knightConfig.Health);
    }
}