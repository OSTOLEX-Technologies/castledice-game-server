using castledice_game_logic.TurnsLogic.TurnSwitchConditions;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.TscConfigCreators;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.TscConfigCreatorsTests;

public class DefaultTscConfigCreatorTests
{
    [Fact]
    public void GetTurnSwitchConditionsConfig_ShouldReturnConfig_WithTscTypesListFromConstructor()
    {
        var expectedList = new List<TscType> { TscType.SwitchByActionPoints };
        var defaultTscConfigCreator = new DefaultTscConfigCreator(expectedList);
        
        var result = defaultTscConfigCreator.GetTurnSwitchConditionsConfig();
        var actualList = result.ConditionsToUse;
        
        Assert.Same(expectedList, actualList);
    }
}