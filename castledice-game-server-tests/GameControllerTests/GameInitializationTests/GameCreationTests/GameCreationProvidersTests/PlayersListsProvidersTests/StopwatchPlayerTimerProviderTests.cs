using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.PlayersListsProvidersTests;

public class StopwatchPlayerTimerProviderTests
{
    [Fact]
    public void GetPlayerTimer_ShouldReturnStopwatchTimer_WithTimeSpanFromProvider()
    {
        var expectedTimeSpan = GetRandomTimeSpan();
        var timeSpanProviderMock = new Mock<IPlayerTimeSpanProvider>();
        timeSpanProviderMock.Setup(x => x.GetTimeSpan()).Returns(expectedTimeSpan);
        var provider = new StopwatchPlayerTimerProvider(timeSpanProviderMock.Object);
        
        var timer = provider.GetPlayerTimer();
        
        Assert.Equal(expectedTimeSpan, timer.GetTimeLeft());
    }
}