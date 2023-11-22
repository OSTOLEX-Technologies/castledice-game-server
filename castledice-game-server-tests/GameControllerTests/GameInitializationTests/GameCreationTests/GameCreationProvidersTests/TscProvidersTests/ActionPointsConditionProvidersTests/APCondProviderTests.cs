using System.Reflection;
using castledice_game_logic.TurnsLogic;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.ActionPointsConditionProviders;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.TscProvidersTests.ActionPointsConditionProvidersTests;

/// <summary>
/// This is a shortened name of the class. The full name is ActionPointsConditionProviderTests.
/// </summary>
public class APCondProviderTests
{
    [Fact]
    public void GetActionPointsCondition_ShouldReturnCondition_WithGivenCurrentPlayerProvider()
    {
        var expectedProvider = new Mock<ICurrentPlayerProvider>().Object;
        var actionPointsConditionProvider = new ActionPointsConditionProvider(expectedProvider);
        
        var condition = actionPointsConditionProvider.GetActionPointsCondition();
        var type = condition.GetType();
        var field = type.GetField("_currentPlayerProvider", BindingFlags.NonPublic | BindingFlags.Instance);
        var actualProvider = field.GetValue(condition);
        
        Assert.Same(expectedProvider, actualProvider);
    }
}