using castledice_game_data_logic.TurnSwitchConditions;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.TscProvidersTests;

public class DefaultTscPresenceConfigProviderTests
{
    [Theory]
    [InlineData(TscType.ActionPoints)]
    [InlineData(TscType.Time)]
    [InlineData(TscType.ActionPoints, TscType.Time)]
    public void GetTscPresenceConfig_ShouldReturnTscPresenceConfig_WithTypesGivenInProviderConstructor(params TscType[] types)
    {
        var expectedTypes = new HashSet<TscType>(types);
        var provider = new DefaultTscPresenceConfigProvider(expectedTypes);
        
        var actualConfig = provider.GetTscPresenceConfig();
        
        Assert.Equal(expectedTypes, actualConfig.PresentConditions);
    }
}