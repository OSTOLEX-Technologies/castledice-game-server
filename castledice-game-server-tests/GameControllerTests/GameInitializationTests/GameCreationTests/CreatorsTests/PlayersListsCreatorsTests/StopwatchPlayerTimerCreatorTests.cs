using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.PlayersListsCreatorsTests;

public class StopwatchPlayerTimerCreatorTests
{
    [Fact]
    public void GetPlayerTimer_ShouldReturnStopwatchTimer_WithTimeSpanFromCreator()
    {
        var expectedTimeSpan = GetRandomTimeSpan();
        var timeSpanCreatorMock = new Mock<IPlayerTimeSpanCreator>();
        timeSpanCreatorMock.Setup(x => x.GetTimeSpan()).Returns(expectedTimeSpan);
        var creator = new StopwatchPlayerTimerCreator(timeSpanCreatorMock.Object);
        
        var timer = creator.GetPlayerTimer();
        
        Assert.Equal(expectedTimeSpan, timer.GetTimeLeft());
    }
}