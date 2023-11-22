using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.TimeConditionProviders;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.TscProvidersTests.TimeConditionProvidersTests;

public class TimeConditionConfigTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Properties_ShouldReturnValues_GivenInConstructor(int turnDuration)
    {
        var config = new TimeConditionConfig(turnDuration);
        
        Assert.Equal(turnDuration, config.TurnDuration);
    }
}