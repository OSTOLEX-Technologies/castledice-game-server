using castledice_game_logic;
using castledice_game_server.GameController;
using castledice_game_server.GameController.PlayersReadiness;
using castledice_game_server.GameController.Timers;
using castledice_game_server.Logging;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.TimersTests;

public class TimersControllerTests
{
    [Fact]
    public void SwitchTimersForPlayers_ShouldBeCalled_IfAllPlayersAreReady()
    {
        var game = GetGame();
        var readinessNotifierMock = new Mock<IGamePlayersReadinessNotifier>();
        readinessNotifierMock.Setup(n => n.NotifyPlayersAreReady(game)).Raises(n => n.PlayersAreReady += null, this, game);
        var readinessNotifier = readinessNotifierMock.Object;
        var timersControllerMock = new TimersControllerBuilder()
        {
            GamePlayersReadinessNotifier = readinessNotifier,
        }.BuildMock();
        var testObject = timersControllerMock.Object; //Forcing constructor call
        
        readinessNotifier.NotifyPlayersAreReady(game);
        
        timersControllerMock.Verify(c => c.SwitchTimersForPlayers(game), Times.Once);
    }
    
    [Fact]
    public void SwitchTimersForPlayers_ShouldBeCalled_IfTurnSwitchedOnGame_AfterGameWasAdded()
    {
        var gameMock = GetGameMock();
        gameMock.Setup(g => g.SwitchTurn()).Raises(g => g.TurnSwitched += null, this, gameMock.Object);
        var game = gameMock.Object;
        var gamesCollectionMock = new Mock<IGamesCollection>();
        gamesCollectionMock.Setup(c => c.AddGame(It.IsAny<int>(), game)).Raises(c => c.GameAdded += null, this, game);
        var gamesCollection = gamesCollectionMock.Object;
        var timersControllerMock = new TimersControllerBuilder()
        {
            GamesCollection = gamesCollection,
        }.BuildMock();
        var testObject = timersControllerMock.Object; //Forcing constructor call
        
        gamesCollection.AddGame(1, game);
        game.SwitchTurn();
        
        timersControllerMock.Verify(c => c.SwitchTimersForPlayers(It.IsAny<Game>()), Times.Once);
    }

    [Fact]
    public void SwitchTimersForPlayers_ShouldNotBeCalled_IfTurnSwitchedOnGame_AfterGameWasRemoved()
    {
        var gameMock = GetGameMock();
        gameMock.Setup(g => g.CheckTurns()).Raises(g => g.TurnSwitched += null, this, gameMock.Object);
        var game = gameMock.Object;
        var gamesCollectionMock = new Mock<IGamesCollection>();
        gamesCollectionMock.Setup(c => c.RemoveGame(It.IsAny<int>())).Returns(true).Raises(c => c.GameRemoved += null, this, game);
        gamesCollectionMock.Setup(c => c.AddGame(It.IsAny<int>(), It.IsAny<Game>())).Raises(c => c.GameAdded += null, this, game);
        var gamesCollection = gamesCollectionMock.Object;
        var timersControllerMock = new TimersControllerBuilder()
        {
            GamesCollection = gamesCollection,
        }.BuildMock();
        
        gamesCollection.AddGame(1, game);
        gamesCollection.RemoveGame(1);
        game.CheckTurns();
        
        timersControllerMock.Verify(c => c.SwitchTimersForPlayers(game), Times.Never);
    }

    private class TimersControllerBuilder
    {
        public IGamePlayersReadinessNotifier GamePlayersReadinessNotifier { get; set; } = new Mock<IGamePlayersReadinessNotifier>().Object;
        public IGamesCollection GamesCollection { get; set; } = new Mock<IGamesCollection>().Object;
        public ITimerSwitchSender TimerSwitchSender { get; set; } = new Mock<ITimerSwitchSender>().Object;
        public ILogger Logger { get; set; } = new Mock<ILogger>().Object;
        
        public TimersController Build()
        {
            return new TimersController(GamePlayersReadinessNotifier, GamesCollection, TimerSwitchSender, Logger);
        }
        
        public Mock<TimersController> BuildMock()
        {
            return new Mock<TimersController>(GamePlayersReadinessNotifier, GamesCollection, TimerSwitchSender, Logger);
        }
    }
}