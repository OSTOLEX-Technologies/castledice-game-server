using castledice_game_logic.TurnsLogic.TurnSwitchConditions;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscConfigProviders;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.TscConfigProvidersTests;

public class DefaultTscConfigProviderTests
{
    [Fact]
    public void GetTurnSwitchConditionsConfig_ShouldReturnConfig_WithTscTypesListFromConstructor()
    {
        var expectedList = new List<TscType> { TscType.SwitchByActionPoints };
        var defaultTscConfigProvider = new DefaultTscConfigProvider(expectedList);
        
        var result = defaultTscConfigProvider.GetTurnSwitchConditionsConfig();
        var actualList = result.ConditionsToUse;
        
        Assert.Same(expectedList, actualList);
    }
}