using castledice_game_server.GameController.PlayersReadiness;

namespace castledice_game_server_tests.GameControllerTests.PlayersReadinessTests;

public class PlayersReadinessTrackerTests
{
    [Theory]
    [InlineData(1, true)]
    [InlineData(2, false)]
    [InlineData(1245, true)]
    public void IsPlayerReady_ShouldReturn_IfPlayerIsReady(int playerId, bool isReady)
    {
        var playersReadinessTracker = new PlayersReadinessTracker();
        playersReadinessTracker.SetPlayerReadiness(playerId, isReady);
        
        var result = playersReadinessTracker.PlayerIsReady(playerId);
        
        Assert.Equal(isReady, result);
    }

    [Fact]
    public void IsPlayerReady_ShouldReturnFalse_IfPlayerReadinessWasNotSetBefore()
    {
        var playerId = 1;
        var playersReadinessTracker = new PlayersReadinessTracker();
        
        var result = playersReadinessTracker.PlayerIsReady(playerId);
        
        Assert.False(result);
    }
}