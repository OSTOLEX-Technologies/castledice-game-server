using System.Reflection;
using castledice_game_logic;
using castledice_game_logic.TurnsLogic;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.TimeConditionProviders;
using Moq;
using ITimer = castledice_game_logic.Time.ITimer;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.TscProvidersTests.TimeConditionProvidersTests;

public class TimeConditionProviderTests
{
    [Fact]
    public void GetTimeCondition_ShouldReturnTimeCondition_WithGivenTimer()
    {
        var configProviderMock = new Mock<ITimeConditionConfigProvider>();
        configProviderMock.Setup(provider => provider.GetTimeConditionConfig()).Returns(new TimeConditionConfig(10));
        var expectedTimer = new Mock<ITimer>().Object;
        var playerTurnsSwitcher = new PlayerTurnsSwitcher(new PlayersList(new List<Player>{GetPlayer(1)}));
        var timeConditionProvider = new TimeConditionProvider(configProviderMock.Object, expectedTimer, playerTurnsSwitcher);
        
        var condition = timeConditionProvider.GetTimeCondition();
        var type = condition.GetType();
        var field = type.GetField("_timer", BindingFlags.NonPublic | BindingFlags.Instance);
        var actualTimer = field.GetValue(condition);
        
        Assert.Same(expectedTimer, actualTimer);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public void GetTimeCondition_ShouldReturnTimeCondition_WithDurationFromConfig(int expectedTurnDuration)
    {
        var playerTurnsSwitcher = new PlayerTurnsSwitcher(new PlayersList(new List<Player>{GetPlayer(1)}));
        var configProviderMock = new Mock<ITimeConditionConfigProvider>();
        configProviderMock.Setup(provider => provider.GetTimeConditionConfig()).Returns(new TimeConditionConfig(expectedTurnDuration));
        var timerMock = new Mock<ITimer>();
        var timeConditionProvider = new TimeConditionProvider(configProviderMock.Object, timerMock.Object, playerTurnsSwitcher);
        
        var condition = timeConditionProvider.GetTimeCondition();
        
        timerMock.Verify(timer => timer.SetDuration(expectedTurnDuration)); //We check if condition has proper duration by checking if it was set to given timer.
    }
    
    [Fact]
    public void GetTimeCondition_ShouldReturnTimeCondition_WithGivenPlayerTurnsSwitcher()
    {
        var configProviderMock = new Mock<ITimeConditionConfigProvider>();
        configProviderMock.Setup(provider => provider.GetTimeConditionConfig()).Returns(new TimeConditionConfig(10));
        var expectedPlayerTurnsSwitcher = new PlayerTurnsSwitcher(new PlayersList(new List<Player>{GetPlayer(1)}));
        var timeConditionProvider = new TimeConditionProvider(configProviderMock.Object, new Mock<ITimer>().Object, expectedPlayerTurnsSwitcher);
        
        var condition = timeConditionProvider.GetTimeCondition();
        var type = condition.GetType();
        var field = type.GetField("_turnsSwitcher", BindingFlags.NonPublic | BindingFlags.Instance);
        var actualPlayerTurnsSwitcher = field.GetValue(condition);
        
        Assert.Same(expectedPlayerTurnsSwitcher, actualPlayerTurnsSwitcher);
    }
}