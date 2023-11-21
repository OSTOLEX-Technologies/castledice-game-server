using castledice_game_data_logic.TurnSwitchConditions;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.TscProvidersTests;

public class TscPresenceConfigTests
{
    [Theory]
    [InlineData(TscType.Time)]
    [InlineData(TscType.Time, TscType.ActionPoints)]
    public void PresentTypes_ShouldReturnValues_FromSetGivenInConstructor(params TscType[] presentTypesArray)
    {
        var expectedPresentTypes = new HashSet<TscType>(presentTypesArray);
        var config = new TscPresenceConfig(expectedPresentTypes);
        
        Assert.Equal(expectedPresentTypes, config.PresentTypes);
    }
    
}