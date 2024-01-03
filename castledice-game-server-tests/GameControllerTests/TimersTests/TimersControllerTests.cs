using castledice_game_logic;
using castledice_game_logic.ActionPointsLogic;
using castledice_game_logic.GameObjects;
using castledice_game_logic.Time;
using castledice_game_server_tests.Builders;
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

    [Fact]
    public void SwitchTimersForPlayers_ShouldStopPreviousPlayerTimer()
    {
        var timer = new Mock<IPlayerTimer>();
        var player = new PlayerBuilder {Timer = timer.Object}.Build();
        var game = GetGameMock();
        game.Setup(g => g.GetPreviousPlayer()).Returns(player);
        var timersController = new TimersControllerBuilder().Build();
        
        timersController.SwitchTimersForPlayers(game.Object);
        
        timer.Verify(t => t.Stop(), Times.Once);
    }

    [Fact]
    public void SwitchTimersForPlayers_ShouldSendTimerSwitchFalse_ForPreviousPlayer_ToAllPlayers()
    {
        var gameMock = GetGameMock();
        var players = GetRandomPlayersList();
        gameMock.Setup(g => g.GetAllPlayers()).Returns(players);
        var previousPlayerIndex = new Random().Next(players.Count);
        var previousPlayer = players[previousPlayerIndex];
        gameMock.Setup(g => g.GetPreviousPlayer()).Returns(previousPlayer);
        var timerSwitchSenderMock = new Mock<ITimerSwitchSender>();
        var timersController = new TimersControllerBuilder()
        {
            TimerSwitchSender = timerSwitchSenderMock.Object,
        }.Build();
        
        timersController.SwitchTimersForPlayers(gameMock.Object);

        foreach (var player in players)
        {
            timerSwitchSenderMock.Verify(s => s.SendTimerSwitch(previousPlayer.Id, It.IsAny<TimeSpan>(), player.Id, false), Times.Once);
        }
    }

    [Fact]
    public void SwitchTimersForPlayers_ShouldSendTimerSwitchWithCorrectTimeLeft()
    {
        var gameMock = GetGameMock();
        var players = GetRandomPlayersList();
        gameMock.Setup(g => g.GetAllPlayers()).Returns(players);
        var previousPlayerIndex = new Random().Next(players.Count);
        var timeLeft = GetRandomTimeSpan();
        var timerMock = new Mock<IPlayerTimer>();
        timerMock.Setup(t => t.GetTimeLeft()).Returns(timeLeft);
        players[previousPlayerIndex] = new PlayerBuilder {Timer = timerMock.Object, Id = players[previousPlayerIndex].Id}.Build();
        var previousPlayer = players[previousPlayerIndex];
        gameMock.Setup(g => g.GetPreviousPlayer()).Returns(previousPlayer);
        var timerSwitchSenderMock = new Mock<ITimerSwitchSender>();
        var timersController = new TimersControllerBuilder()
        {
            TimerSwitchSender = timerSwitchSenderMock.Object,
        }.Build();
        
        timersController.SwitchTimersForPlayers(gameMock.Object);

        foreach (var player in players)
        {
            timerSwitchSenderMock.Verify(s => s.SendTimerSwitch(previousPlayer.Id, timeLeft, player.Id, false), Times.Once);
        }
    }

    [Fact]
    public void SwitchTimersForPlayers_ShouldStartTimer_ForCurrentPlayer()
    {
        var timerMock = new Mock<IPlayerTimer>();
        var player = new PlayerBuilder {Timer = timerMock.Object}.Build();
        var gameMock = GetGameMock();
        gameMock.Setup(g => g.GetCurrentPlayer()).Returns(player);
        gameMock.Setup(g => g.GetPreviousPlayer()).Returns(GetPlayer());
        var timersController = new TimersControllerBuilder().Build();
        
        timersController.SwitchTimersForPlayers(gameMock.Object);
        
        timerMock.Verify(t => t.Start(), Times.Once);
    }
    
    [Fact]
    public void SwitchTimersForPlayers_ShouldSendTimerSwitchTrue_ForCurrentPlayer_ToAllPlayers()
    {
        var gameMock = GetGameMock();
        var players = GetRandomPlayersList();
        gameMock.Setup(g => g.GetAllPlayers()).Returns(players);
        var currentPlayerIndex = new Random().Next(players.Count);
        var currentPlayer = players[currentPlayerIndex];
        gameMock.Setup(g => g.GetCurrentPlayer()).Returns(currentPlayer);
        gameMock.Setup(g => g.GetPreviousPlayer()).Returns(GetPlayer());
        var timerSwitchSenderMock = new Mock<ITimerSwitchSender>();
        var timersController = new TimersControllerBuilder()
        {
            TimerSwitchSender = timerSwitchSenderMock.Object,
        }.Build();
        
        timersController.SwitchTimersForPlayers(gameMock.Object);

        foreach (var player in players)
        {
            timerSwitchSenderMock.Verify(s => s.SendTimerSwitch(currentPlayer.Id, It.IsAny<TimeSpan>(), player.Id, true), Times.Once);
        }
    }
    
    [Fact]
    public void SwitchTimersForPlayers_ShouldSendCorrectTimeLeft_ForCurrentPlayer_ToAllPlayers()
    {
        var gameMock = GetGameMock();
        var players = GetRandomPlayersList();
        gameMock.Setup(g => g.GetAllPlayers()).Returns(players);
        var currentPlayerIndex = new Random().Next(players.Count);
        var timeLeft = GetRandomTimeSpan();
        var timerMock = new Mock<IPlayerTimer>();
        timerMock.Setup(t => t.GetTimeLeft()).Returns(timeLeft);
        players[currentPlayerIndex] = new PlayerBuilder {Timer = timerMock.Object, Id = players[currentPlayerIndex].Id}.Build();
        var currentPlayer = players[currentPlayerIndex];
        gameMock.Setup(g => g.GetCurrentPlayer()).Returns(currentPlayer);
        gameMock.Setup(g => g.GetPreviousPlayer()).Returns(GetPlayer());
        var timerSwitchSenderMock = new Mock<ITimerSwitchSender>();
        var timersController = new TimersControllerBuilder()
        {
            TimerSwitchSender = timerSwitchSenderMock.Object,
        }.Build();
        
        timersController.SwitchTimersForPlayers(gameMock.Object);

        foreach (var player in players)
        {
            timerSwitchSenderMock.Verify(s => s.SendTimerSwitch(currentPlayer.Id, timeLeft, player.Id, true), Times.Once);
        }
    }
    
    [Fact]
    public void SwitchTimersForPlayers_ShouldLogExceptionMessages_FromTimerSwitchSender()
    {
        var loggerMock = new Mock<ILogger>();
        var timerSwitchSenderMock = new Mock<ITimerSwitchSender>();
        var message = "Test exception";
        timerSwitchSenderMock.Setup(s =>
                s.SendTimerSwitch(It.IsAny<int>(), It.IsAny<TimeSpan>(), It.IsAny<int>(), It.IsAny<bool>()))
            .Throws(new Exception(message));
        var timersController = new TimersControllerBuilder()
        {
            TimerSwitchSender = timerSwitchSenderMock.Object,
            Logger = loggerMock.Object,
        }.Build();
        
        timersController.SwitchTimersForPlayers(GetGame());
        
        loggerMock.Verify(l => l.Error(message), Times.Once);
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