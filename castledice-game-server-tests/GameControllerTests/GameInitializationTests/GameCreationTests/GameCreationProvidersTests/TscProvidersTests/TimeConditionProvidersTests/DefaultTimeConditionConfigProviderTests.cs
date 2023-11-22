using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.TimeConditionProviders;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.TscProvidersTests.TimeConditionProvidersTests;

public class DefaultTimeConditionConfigProviderTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetTimeConditionConfig_ShouldReturnTimeConditionConfig_WithTurnDurationGivenInConstructor(int turnDuration)
    {
        var provider = new DefaultTimeConditionConfigProvider(turnDuration);
        
        var actualConfig = provider.GetTimeConditionConfig();
        
        Assert.Equal(turnDuration, actualConfig.TurnDuration);
    }
}