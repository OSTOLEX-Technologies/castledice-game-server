using castledice_game_data_logic.TurnSwitchConditions;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.TscProvidersTests;

public class TscPresenceConfigTests
{
    [Theory]
    [InlineData(TscType.Time)]
    [InlineData(TscType.Time, TscType.ActionPoints)]
    public void PresentConditions_ShouldReturnValues_FromSetGivenInConstructor(params TscType[] presentTypesArray)
    {
        var expectedPresentTypes = new HashSet<TscType>(presentTypesArray);
        var configProvider = new DefaultTscPresenceConfigProvider(expectedPresentTypes);
        
        var config = configProvider.GetTscPresenceConfig();
        
        Assert.Equal(expectedPresentTypes, config.PresentConditions);
    }
    
}