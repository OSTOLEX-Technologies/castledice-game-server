using castledice_game_logic;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.GameObjects;
using castledice_game_logic.Math;
using castledice_game_logic.MovesLogic;
using castledice_game_server_tests.TestImplementations;
using castledice_game_server.GameController;
using castledice_game_server.GameController.GameOver;
using castledice_game_server.GameService;
using ILogger = castledice_game_server.Logging.ILogger;
using static castledice_game_server_tests.ObjectCreationUtility;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameOverTests;

public class GameOverControllerTests
{
    public class TestGame : Game
    {
        public TestGame(List<Player> players, BoardConfig boardConfig, PlaceablesConfig placeablesConfig, TurnSwitchConditionsConfig turnSwitchConditionsConfig) : base(players, boardConfig, placeablesConfig, turnSwitchConditionsConfig)
        {
        }
        
        public void ForceWin(Player winner)
        {
            OnWin(winner);
        }
        
        public void ForceDraw()
        {
            OnDraw();
        }

        public void InvokeMoveApplied(AbstractMove move)
        {
            OnMoveApplied(move);
        }
    }
    
    [Fact]
    //This test also checks if OnGameAdded is called and subscription is made in it.
    public void OnWin_ShouldCallSaveWin_WithGameAndWinner()
    {
        var gameMock = GetTestGameMock();
        var expectedGame = gameMock.Object;
        var expectedWinner = GetPlayer(1);
        var gamesCollection = new TestGamesCollection();
        var gameOverController = new Mock<GameOverController>(gamesCollection, new Mock<IGameSavingService>().Object, new Mock<IHistoryProvider>().Object, new Mock<ILogger>().Object);
        var testObject = gameOverController.Object;
        gamesCollection.AddGame(1, expectedGame);//This should force OnGameAdded to be called
        
        expectedGame.ForceWin(expectedWinner);
        
        gameOverController.Verify(x => x.SaveWin(expectedGame, expectedWinner), Times.Once);
    }

    [Fact]
    public void OnDraw_ShouldCallSaveDraw_WithGame()
    {
        var gameMock = GetTestGameMock();
        var expectedGame = gameMock.Object;
        var gamesCollection = new TestGamesCollection();
        var gameOverControllerMock = new Mock<GameOverController>(gamesCollection, new Mock<IGameSavingService>().Object, new Mock<IHistoryProvider>().Object, new Mock<ILogger>().Object);
        var testObject = gameOverControllerMock.Object;
        gamesCollection.AddGame(1, expectedGame);//This should force OnGameAdded to be called
        
        expectedGame.ForceDraw();
        
        gameOverControllerMock.Verify(x => x.SaveDraw(expectedGame), Times.Once);
    }
    
    [Fact]
    public async void SaveWin_ShouldUnsubscribeFromGameEvents()
    {
        var gameMock = GetTestGameMock();
        var expectedGame = gameMock.Object;
        var gamesCollection = new TestGamesCollection();
        var gameOverControllerMock = new Mock<GameOverController>(gamesCollection, new Mock<IGameSavingService>().Object, new Mock<IHistoryProvider>().Object, new Mock<ILogger>().Object);
        gameOverControllerMock.CallBase = true;
        var controller = gameOverControllerMock.Object;
        gamesCollection.AddGame(1, expectedGame);//This should force OnGameAdded to be called

        await controller.SaveWin(expectedGame, GetPlayer(1));
        expectedGame.ForceWin(GetPlayer(1));
        expectedGame.ForceDraw();
        
        gameOverControllerMock.Verify(x => x.SaveWin(It.IsAny<Game>(), It.IsAny<Player>()), Times.Once);
        gameOverControllerMock.Verify(x => x.SaveDraw(It.IsAny<Game>()), Times.Never);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(13)]
    public async void SaveWin_ShouldRemoveGame_FromCollection(int gameId)
    {
        var gameMock = GetTestGameMock();
        var expectedGame = gameMock.Object;
        var gamesCollectionMock = new Mock<IGamesCollection>();
        gamesCollectionMock.Setup(g => g.GetGameId(expectedGame)).Returns(gameId);
        var gameOverController = new GameOverControllerBuilder()
        {
            ActiveGames = gamesCollectionMock.Object
        }.Build();
        
        await gameOverController.SaveWin(expectedGame, GetPlayer(1));

        gamesCollectionMock.Verify(g => g.RemoveGame(gameId), Times.Once);
    }

    [Theory]
    [InlineData(1, 1, "somehistory")]
    [InlineData(2, 2, "someotherhistory")]
    [InlineData(13, 3, "someotherhistory")]
    public async void SaveWin_ShouldCallSaveGameEndAsyncOnService_WithGameAndWinnerIdAndHistoryFromProvider(int gameId,
        int winnerId, string history)
    {
        var gameMock = GetTestGameMock();
        var expectedGame = gameMock.Object;
        var gamesCollectionMock = new Mock<IGamesCollection>();
        gamesCollectionMock.Setup(g => g.GetGameId(expectedGame)).Returns(gameId);
        var gameSavingServiceMock = new Mock<IGameSavingService>();
        var historyProviderMock = new Mock<IHistoryProvider>();
        historyProviderMock.Setup(h => h.GetGameHistory(expectedGame)).Returns(history);
        var gameOverController = new GameOverControllerBuilder()
        {
            ActiveGames = gamesCollectionMock.Object,
            GameSavingService = gameSavingServiceMock.Object,
            HistoryProvider = historyProviderMock.Object
        }.Build();
        
        await gameOverController.SaveWin(expectedGame, GetPlayer(winnerId));
        
        gameSavingServiceMock.Verify(g => g.SaveGameEndAsync(gameId, history, winnerId), Times.Once);
    }
    
    [Theory]
    [InlineData("some error")]
    [InlineData("some other error")]
    [InlineData("some other error")]
    public async void SaveWin_ShouldLogError_IfExceptionIsThrown(string message)
    {
        var gameMock = GetTestGameMock();
        var expectedGame = gameMock.Object;
        var gamesCollectionMock = new Mock<IGamesCollection>();
        gamesCollectionMock.Setup(g => g.GetGameId(expectedGame)).Returns(1);
        var gameSavingServiceMock = new Mock<IGameSavingService>();
        gameSavingServiceMock.Setup(g => g.SaveGameEndAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .Throws(new Exception(message));
        var loggerMock = new Mock<ILogger>();
        var gameOverController = new GameOverControllerBuilder()
        {
            ActiveGames = gamesCollectionMock.Object,
            GameSavingService = gameSavingServiceMock.Object,
            Logger = loggerMock.Object
        }.Build();
        
        await gameOverController.SaveWin(expectedGame, GetPlayer(1));
        
        loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async void SaveDraw_ShouldUnsubscribeFromGameEvents()
    {
        var gameMock = GetTestGameMock();
        var expectedGame = gameMock.Object;
        var gamesCollection = new TestGamesCollection();
        var gameOverControllerMock = new Mock<GameOverController>(gamesCollection, new Mock<IGameSavingService>().Object, new Mock<IHistoryProvider>().Object, new Mock<ILogger>().Object);
        gameOverControllerMock.CallBase = true;
        var controller = gameOverControllerMock.Object;
        gamesCollection.AddGame(1, expectedGame);//This should force OnGameAdded to be called

        await controller.SaveDraw(expectedGame);
        expectedGame.ForceWin(GetPlayer(1));
        expectedGame.ForceDraw();
        
        gameOverControllerMock.Verify(x => x.SaveWin(It.IsAny<Game>(), It.IsAny<Player>()), Times.Never);
        gameOverControllerMock.Verify(x => x.SaveDraw(It.IsAny<Game>()), Times.Once);
    }

    [Theory]
    [InlineData(1, "somehistory")]
    [InlineData(2, "someotherhistory")]
    [InlineData(13, "someotherhistory")]
    public async void SaveDraw_ShouldCallSaveGameEndAsyncOnService_WithGameIdAndHistoryFromProviderAndNullWinnerId(
        int gameId, string history)
    {
        var gameMock = GetTestGameMock();
        var expectedGame = gameMock.Object;
        var gamesCollectionMock = new Mock<IGamesCollection>();
        gamesCollectionMock.Setup(g => g.GetGameId(expectedGame)).Returns(gameId);
        var gameSavingServiceMock = new Mock<IGameSavingService>();
        var historyProviderMock = new Mock<IHistoryProvider>();
        historyProviderMock.Setup(h => h.GetGameHistory(expectedGame)).Returns(history);
        var gameOverController = new GameOverControllerBuilder()
        {
            ActiveGames = gamesCollectionMock.Object,
            GameSavingService = gameSavingServiceMock.Object,
            HistoryProvider = historyProviderMock.Object
        }.Build();
        
        await gameOverController.SaveDraw(expectedGame);
        
        gameSavingServiceMock.Verify(g => g.SaveGameEndAsync(gameId, history, null), Times.Once);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(13)]
    public async void SaveDraw_ShouldRemoveGame_FromCollection(int gameId)
    {
        var gameMock = GetTestGameMock();
        var expectedGame = gameMock.Object;
        var gamesCollectionMock = new Mock<IGamesCollection>();
        gamesCollectionMock.Setup(g => g.GetGameId(expectedGame)).Returns(gameId);
        var gameOverController = new GameOverControllerBuilder()
        {
            ActiveGames = gamesCollectionMock.Object
        }.Build();
        
        await gameOverController.SaveDraw(expectedGame);

        gamesCollectionMock.Verify(g => g.RemoveGame(gameId), Times.Once);
    }

    [Theory]
    [InlineData("some error")]
    [InlineData("some other error")]
    [InlineData("some other error")]
    public async void SaveDraw_ShouldLogError_IfExceptionIsThrown(string message)
    {
        var gameMock = GetTestGameMock();
        var expectedGame = gameMock.Object;
        var gamesCollectionMock = new Mock<IGamesCollection>();
        gamesCollectionMock.Setup(g => g.GetGameId(expectedGame)).Returns(1);
        var gameSavingServiceMock = new Mock<IGameSavingService>();
        gameSavingServiceMock.Setup(g => g.SaveGameEndAsync(It.IsAny<int>(), It.IsAny<string>(), null))
            .Throws(new Exception(message));
        var loggerMock = new Mock<ILogger>();
        var gameOverController = new GameOverControllerBuilder()
        {
            ActiveGames = gamesCollectionMock.Object,
            GameSavingService = gameSavingServiceMock.Object,
            Logger = loggerMock.Object
        }.Build();
        
        await gameOverController.SaveDraw(expectedGame);
        
        loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }
    
    private class GameOverControllerBuilder 
    {
        public IGamesCollection ActiveGames { get; set; } = new Mock<IGamesCollection>().Object;
        public IGameSavingService GameSavingService { get; set; } = new Mock<IGameSavingService>().Object;
        public IHistoryProvider HistoryProvider { get; set; } = new Mock<IHistoryProvider>().Object;
        public ILogger Logger { get; set; } = new Mock<ILogger>().Object;
        
        public GameOverController Build()
        {
            return new GameOverController(ActiveGames, GameSavingService, HistoryProvider, Logger);
        }
    }

    private static Mock<TestGame> GetTestGameMock()
    {
        var player = GetPlayer(1);
        var secondPlayer = GetPlayer(2);
        var playersList = new List<Player> { player, secondPlayer };
        var gameMock = new Mock<TestGame>(playersList, GetBoardConfig(new Dictionary<Player, Vector2Int>
        {
            {player, (0, 0)},
            {secondPlayer, (9, 9)}
        }), GetPlaceablesConfig(), GetTurnSwitchConditionsConfig());
        gameMock.Setup(x => x.GetPlayer(It.IsAny<int>())).Returns(player);
        gameMock.Setup(x => x.GetAllPlayers()).Returns(playersList);
        gameMock.Setup(x => x.GetAllPlayersIds()).Returns(new List<int> { 1, 2 });
        gameMock.Setup(g => g.GetCurrentPlayer()).Returns(GetPlayer(1));
        return gameMock;
    }
}