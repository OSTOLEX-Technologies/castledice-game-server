using castledice_game_logic;
using castledice_game_server.GameController.PlayersReadiness;
using static castledice_game_server_tests.ObjectCreationUtility;

namespace castledice_game_server_tests.GameControllerTests.PlayersReadinessTests;

public class GamePlayersReadinessNotifierTests
{
    [Fact]
    public void NotifyPlayersAreReady_ShouldRaisePlayersAreReadyEvent()
    {
        var game = GetGame();
        var notifier = new GamePlayersReadinessNotifier();
        var eventRaised = false;
        notifier.PlayersAreReady += (sender, game) => eventRaised = true;
        
        notifier.NotifyPlayersAreReady(game);
        
        Assert.True(eventRaised);
    }
    
    [Fact]
    public void NotifyPlayersAreReady_ShouldPassGameToPlayersAreReadyEvent()
    {
        var game = GetGame();
        var notifier = new GamePlayersReadinessNotifier();
        Game? passedGame = null;
        notifier.PlayersAreReady += (sender, game) => passedGame = game;
        
        notifier.NotifyPlayersAreReady(game);
        
        Assert.Equal(game, passedGame);
    }
}