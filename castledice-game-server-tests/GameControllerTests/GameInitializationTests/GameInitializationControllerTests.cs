using castledice_game_data_logic;
using castledice_game_logic;
using castledice_game_server.GameController;
using castledice_game_server.GameController.GameInitialization;
using castledice_game_server.GameController.GameInitialization.GameCreation;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation;
using castledice_game_server.GameService;
using castledice_game_server.Logging;
using castledice_game_server.NetworkManager;
using Moq;
using static castledice_game_server_tests.ObjectCreationUtility;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests;

public class GameInitializationControllerTests
{
    [Fact]
    public async void InitializeGameAsync_ShouldCallCreateGame_OnGivenGameCreator()
    {
        var gameCreatorMock = GetGameCreatorMock();
        var gameCreationController = new GameCreationControllerBuilder
        {
            GameCreator = gameCreatorMock.Object
        }.Build();
        var playersIds = new List<int> { 1, 2 };

        await gameCreationController.InitializeGameAsync(playersIds);

        gameCreatorMock.Verify(gc => gc.CreateGame(playersIds), Times.Once);
    }


    [Fact]
    public async void CreatedGame_ShouldBePassedToCreateGameStartData_OnGivenGameStartDataCreator()
    {
        var gameStartDataCreatorMock = GetGameStartDataCreatorMock();
        var createdGame = GetGame();
        var gameCreationController = new GameCreationControllerBuilder
        {
            GameCreator = GetGameCreatorMock(createdGame).Object,
            GameStartDataCreator = gameStartDataCreatorMock.Object
        }.Build();

        await gameCreationController.InitializeGameAsync(new List<int> { 1, 2 });

        gameStartDataCreatorMock.Verify(creator => creator.CreateGameStartData(createdGame), Times.Once);
    }

    [Fact]
    public async void CreatedGameStartData_ShouldBePassedToSaveGameStartAsync_OnGivenGameSavingService()
    {
        var gameSavingServiceMock = new Mock<IGameSavingService>();
        var createdGameStartData = GetGameStartData();
        var gameStartDataCreatorMock = GetGameStartDataCreatorMock(createdGameStartData);
        var gameCreationController = new GameCreationControllerBuilder
        {
            GameSavingService = gameSavingServiceMock.Object,
            GameStartDataCreator = gameStartDataCreatorMock.Object
        }.Build();

        await gameCreationController.InitializeGameAsync(new List<int> { 1, 2 });

        gameSavingServiceMock.Verify(saver => saver.SaveGameStartAsync(createdGameStartData), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void CreatedGame_ShouldBeAdded_ToGivenGamesCollectionWithAppropriateId(int gameId)
    {
        var gameToCreate = GetGame();
        var gameCreator = GetGameCreatorMock(gameToCreate).Object;
        var gamesCollectionMock = new Mock<IGamesCollection>();
        var savingServiceMock = new Mock<IGameSavingService>();
        savingServiceMock.Setup(saver => saver.SaveGameStartAsync(It.IsAny<GameStartData>())).ReturnsAsync(gameId);
        var gameCreationController = new GameCreationControllerBuilder
        {
            GameCreator = gameCreator,
            ActiveGamesCollection = gamesCollectionMock.Object,
            GameSavingService = savingServiceMock.Object
        }.Build();

        await gameCreationController.InitializeGameAsync(new List<int> { 1, 2 });

        gamesCollectionMock.Verify(collection => collection.AddGame(gameId, gameToCreate), Times.Once);
    }

    [Fact]
    public async void CreatedGameStartData_ShouldBePassedToSendGameStartData_OnGivenGameStartDataSender()
    {
        var gameStartDataSenderMock = new Mock<IGameStartDataSender>();
        var createdGameStartData = GetGameStartData();
        var gameCreationController = new GameCreationControllerBuilder
        {
            GameStartDataCreator = GetGameStartDataCreatorMock(createdGameStartData).Object,
            GameStartDataSender = gameStartDataSenderMock.Object
        }.Build();

        await gameCreationController.InitializeGameAsync(new List<int> { 1, 2 });

        gameStartDataSenderMock.Verify(sender => sender.SendGameStartData(createdGameStartData), Times.Once);
    }

    [Theory]
    [InlineData("some message")]
    [InlineData("another message")]
    [InlineData("yet another message")]
    public async void InitializeGameAsync_ShouldLogCaughtExceptions(string message)
    {
        var gameStartDataCreatorMock = new Mock<IGameStartDataCreator>();
        gameStartDataCreatorMock.Setup(creator => creator.CreateGameStartData(It.IsAny<Game>())).Throws(new Exception(message));
        var loggerMock = new Mock<ILogger>();
        var gameCreationController = new GameCreationControllerBuilder
        {
            GameStartDataCreator = gameStartDataCreatorMock.Object,
            Logger = loggerMock.Object
        }.Build();
        
        await gameCreationController.InitializeGameAsync(new List<int> { 1, 2 });
        
        loggerMock.Verify(logger => logger.Error(message), Times.Once);
    }

public class GameCreationControllerBuilder
    {
        public IGameSavingService GameSavingService { get; set; } = new Mock<IGameSavingService>().Object;
        public IGamesCollection ActiveGamesCollection { get; set; } = new Mock<IGamesCollection>().Object;
        public IGameStartDataSender GameStartDataSender { get; set; } = new Mock<IGameStartDataSender>().Object;
        public IGameCreator GameCreator { get; set; } = GetGameCreatorMock().Object;
        public IGameStartDataCreator GameStartDataCreator { get; set; } = GetGameStartDataCreatorMock().Object;
        public ILogger Logger { get; set; } = new Mock<ILogger>().Object;
        
        public GameInitializationController Build()
        {
            return new GameInitializationController(GameSavingService, ActiveGamesCollection, GameStartDataSender, GameCreator, GameStartDataCreator, Logger);
        }
    }

    private static Mock<IGameCreator> GetGameCreatorMock()
    {
        var gameCreatorMock = new Mock<IGameCreator>();
        gameCreatorMock.Setup(gc => gc.CreateGame(It.IsAny<List<int>>())).Returns(GetGame());
        return gameCreatorMock;
    }
    
    private static Mock<IGameCreator> GetGameCreatorMock(Game gameToReturn)
    {
        var gameCreatorMock = new Mock<IGameCreator>();
        gameCreatorMock.Setup(gc => gc.CreateGame(It.IsAny<List<int>>())).Returns(gameToReturn);
        return gameCreatorMock;
    }

    private static Mock<IGameStartDataCreator> GetGameStartDataCreatorMock()
    {
        var gameStartDataCreatorMock = new Mock<IGameStartDataCreator>();
        gameStartDataCreatorMock.Setup(creator => creator.CreateGameStartData(It.IsAny<Game>())).Returns(GetGameStartData());
        return gameStartDataCreatorMock;
    }
    
    private static Mock<IGameStartDataCreator> GetGameStartDataCreatorMock(GameStartData createdGameStartData)
    {
        var gameStartDataCreatorMock = new Mock<IGameStartDataCreator>();
        gameStartDataCreatorMock.Setup(creator => creator.CreateGameStartData(It.IsAny<Game>())).Returns(createdGameStartData);
        return gameStartDataCreatorMock;
    }
}