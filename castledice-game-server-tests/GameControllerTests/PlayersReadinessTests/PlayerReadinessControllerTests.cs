using castledice_game_logic;
using castledice_game_server.Auth;
using static castledice_game_server_tests.ObjectCreationUtility;
using castledice_game_server.GameController.Moves;
using castledice_game_server.GameController.PlayersReadiness;
using castledice_game_server.Logging;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.PlayersReadinessTests;

public class PlayerReadinessControllerTests
{

    [Theory]
    [InlineData("token", 1)]
    [InlineData("token2", 2)]
    [InlineData("aba134fadfe", 1245)]
    public async void SetPlayerReadyAsync_ShouldPassIdFromRetriever_ToReadinessTracker(string token, int playerId)
    {
        var readinessTrackerMock = new Mock<IPlayersReadinessTracker>();
        var idRetrieverMock = new Mock<IIdRetriever>();
        idRetrieverMock.Setup(retriever => retriever.RetrievePlayerIdAsync(token)).ReturnsAsync(playerId);
        var controller = new PlayerReadinessControllerBuilder()
        {
            IdRetriever = idRetrieverMock.Object,
            PlayersReadinessTracker = readinessTrackerMock.Object
        }.Build();
        
        await controller.SetPlayerReadyAsync(token);
        
        readinessTrackerMock.Verify(tracker => tracker.SetPlayerReadiness(playerId, true));
    }

    [Theory]
    [InlineData("token", 1, 3)]
    [InlineData("token2", 2, 3, 4, 5)]
    [InlineData("aba134fadfe", 1245, 1, 2, 3, 4, 5, 6, 7, 8, 9)]
    public async void SetPlayerReadyAsync_ShouldCheckIfAllPlayersAreReady_InGameForRetrievedPlayerId(string token,
        int playerId, params int[] otherPlayersIds)
    {
        var gameForPlayerProviderMock = GetGameForPlayerProviderMock();
        var gameMock = GetGameMock();
        gameForPlayerProviderMock.Setup(provider => provider.GetGame(playerId)).Returns(gameMock.Object);
        gameMock.Setup(game => game.GetAllPlayersIds()).Returns(new List<int>(otherPlayersIds));
        var readinessTrackerMock = new Mock<IPlayersReadinessTracker>();
        readinessTrackerMock.Setup(tracker => tracker.PlayerIsReady(It.IsAny<int>())).Returns(true);
        var idRetrieverMock = new Mock<IIdRetriever>();
        idRetrieverMock.Setup(retriever => retriever.RetrievePlayerIdAsync(token)).ReturnsAsync(playerId);
        var controller = new PlayerReadinessControllerBuilder()
        {
            IdRetriever = idRetrieverMock.Object,
            GameForPlayerProvider = gameForPlayerProviderMock.Object,
            PlayersReadinessTracker = readinessTrackerMock.Object
        }.Build();
        
        await controller.SetPlayerReadyAsync(token);

        foreach (var id in otherPlayersIds)
        {
            readinessTrackerMock.Verify(tracker => tracker.PlayerIsReady(id), Times.Once);
        }
    }
    
    [Fact]
    public async void SetPlayerReadyAsync_ShouldInvokePlayersReadinessNotifierWithGame_IfAllPlayersAreReadyInGame()
    {
        var gameForPlayerProviderMock = GetGameForPlayerProviderMock();
        var game = GetGameMock().Object;
        gameForPlayerProviderMock.Setup(provider => provider.GetGame(It.IsAny<int>())).Returns(game);
        var readinessTrackerMock = new Mock<IPlayersReadinessTracker>();
        readinessTrackerMock.Setup(tracker => tracker.PlayerIsReady(It.IsAny<int>())).Returns(true);
        var notifierMock = new Mock<IGamePlayersReadinessNotifier>();
        var controller = new PlayerReadinessControllerBuilder()
        {
            GameForPlayerProvider = gameForPlayerProviderMock.Object,
            PlayersReadinessTracker = readinessTrackerMock.Object,
            GamePlayersReadinessNotifier = notifierMock.Object
        }.Build();
        
        await controller.SetPlayerReadyAsync("sometoken");
        
        notifierMock.Verify(notifier => notifier.NotifyPlayersAreReady(game), Times.Once);
    }

    [Theory]
    [InlineData("message")]
    [InlineData("another message")]
    [InlineData("some message")]
    public async void SetPlayerReadyAsync_ShouldLogExceptions_IfAnyThrown(string message)
    {
        var idRetrieverMock = new Mock<IIdRetriever>();
        idRetrieverMock.Setup(retriever => retriever.RetrievePlayerIdAsync(It.IsAny<string>())).ThrowsAsync(new Exception(message));
        var loggerMock = new Mock<ILogger>();
        var controller = new PlayerReadinessControllerBuilder()
        {
            IdRetriever = idRetrieverMock.Object,
            Logger = loggerMock.Object
        }.Build();
        
        await controller.SetPlayerReadyAsync("sometoken");
        
        loggerMock.Verify(logger => logger.Error(message));
    }

    [Theory]
    [InlineData(1, 2, 3, 4, 5)]
    [InlineData(2, 1, 3, 4, 5)]
    public async void SetPlayerReadyAsync_ShouldNotInvokePlayerReadinessNotifier_IfSomePlayerAreNotReady(
        int notReadyPlayer, params int[] readyPlayers)
    {
        var gameForPlayerProviderMock = GetGameForPlayerProviderMock();
        var gameMock = GetGameMock();
        gameForPlayerProviderMock.Setup(provider => provider.GetGame(It.IsAny<int>())).Returns(gameMock.Object);
        gameMock.Setup(game => game.GetAllPlayersIds()).Returns(new List<int>(readyPlayers){notReadyPlayer});
        var readinessTrackerMock = new Mock<IPlayersReadinessTracker>();
        foreach (var readyPlayerId in readyPlayers)
        {
            readinessTrackerMock.Setup(tracker => tracker.PlayerIsReady(readyPlayerId)).Returns(true);
        }
        readinessTrackerMock.Setup(tracker => tracker.PlayerIsReady(notReadyPlayer)).Returns(false);
        var notifierMock = new Mock<IGamePlayersReadinessNotifier>();
        var controller = new PlayerReadinessControllerBuilder()
        {
            GameForPlayerProvider = gameForPlayerProviderMock.Object,
            PlayersReadinessTracker = readinessTrackerMock.Object,
            GamePlayersReadinessNotifier = notifierMock.Object
        }.Build();
        
        await controller.SetPlayerReadyAsync("sometoken");
        
        notifierMock.Verify(notifier => notifier.NotifyPlayersAreReady(It.IsAny<Game>()), Times.Never);
    }

    private class PlayerReadinessControllerBuilder
    {
        public IIdRetriever IdRetriever { get; set; } = new Mock<IIdRetriever>().Object;
        public IGameForPlayerProvider GameForPlayerProvider { get; set; } = GetGameForPlayerProviderMock().Object;
        public IPlayersReadinessTracker PlayersReadinessTracker { get; set; } = new Mock<IPlayersReadinessTracker>().Object;
        public IGamePlayersReadinessNotifier GamePlayersReadinessNotifier { get; set; } = new Mock<IGamePlayersReadinessNotifier>().Object;
        public ILogger Logger { get; set; } = new Mock<ILogger>().Object;
        
        public PlayerReadinessController Build()
        {
            return new PlayerReadinessController(IdRetriever, GameForPlayerProvider, PlayersReadinessTracker, GamePlayersReadinessNotifier, Logger);
        }
    }
}