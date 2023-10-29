using castledice_game_data_logic;
using castledice_game_logic;
using castledice_game_server.GameController;
using castledice_game_server.GameController.GameInitialization;
using castledice_game_server.GameController.GameInitialization.GameCreation;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation;
using castledice_game_server.GameDataSaver;
using castledice_game_server.NetworkManager;
using Moq;
using static castledice_game_server_tests.ObjectCreationUtility;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests;

public class GameInitializationControllerTests
{
    [Fact]
    public void InitializeGame_ShouldCallCreateGame_OnGivenGameCreator()
    {
        var gameCreatorMock = GetGameCreatorMock();
        var gameCreationController = new GameCreationControllerBuilder
        {
            GameCreator = gameCreatorMock.Object
        }.Build();
        var playersIds = new List<int> {1, 2};
        
        gameCreationController.InitializeGame(playersIds);
        
        gameCreatorMock.Verify(gc => gc.CreateGame(playersIds), Times.Once);
    }

    [Fact]
    public void CreatedGame_ShouldBeAdded_ToGivenActiveGamesCollection()
    {
        var gameToCreate = GetGame();
        var gameCreator = GetGameCreatorMock(gameToCreate).Object;
        var activeGamesCollection = new ActiveGamesCollection();
        var gameCreationController = new GameCreationControllerBuilder
        {
            GameCreator = gameCreator,
            ActiveGamesCollection = activeGamesCollection
        }.Build();
        
        gameCreationController.InitializeGame(new List<int>{1, 2});
        
        Assert.Same(gameToCreate, activeGamesCollection.ActiveGames[0]);
    }

    [Fact]
    public void CreatedGame_ShouldBePassedToCreateGameStartData_OnGivenGameStartDataCreator()
    {
        var gameStartDataCreatorMock = GetGameStartDataCreatorMock();
        var createdGame = GetGame();
        var gameCreationController = new GameCreationControllerBuilder
        {
            GameCreator = GetGameCreatorMock(createdGame).Object,
            GameStartDataCreator = gameStartDataCreatorMock.Object
        }.Build();
        
        gameCreationController.InitializeGame(new List<int>{1, 2});
        
        gameStartDataCreatorMock.Verify(creator => creator.CreateGameStartData(createdGame), Times.Once);
    }

    [Fact]
    public void CreatedGame_ShouldBePassedToSaveGameStart_OnGivenGameSavingService()
    {
        var gameSavingServiceMock = new Mock<IGameSavingService>();
        var createdGame = GetGame();
        var gameCreationController = new GameCreationControllerBuilder
        {
            GameCreator = GetGameCreatorMock(createdGame).Object,
            GameSavingService = gameSavingServiceMock.Object
        }.Build();
        
        gameCreationController.InitializeGame(new List<int>{1, 2});
        
        gameSavingServiceMock.Verify(saver => saver.SaveGameStart(createdGame), Times.Once);
    }
    
    [Fact]
    public void CreatedGameStartData_ShouldBePassedToSendGameStartData_OnGivenGameStartDataSender()
    {
        var gameStartDataSenderMock = new Mock<IGameStartDataSender>();
        var createdGameStartData = GetGameStartData();
        var gameCreationController = new GameCreationControllerBuilder
        {
            GameStartDataCreator = GetGameStartDataCreatorMock(createdGameStartData).Object,
            GameStartDataSender = gameStartDataSenderMock.Object
        }.Build();
        
        gameCreationController.InitializeGame(new List<int>{1, 2});
        
        gameStartDataSenderMock.Verify(sender => sender.SendGameStartData(createdGameStartData), Times.Once);
    }

    public class GameCreationControllerBuilder
    {
        public IGameSavingService GameSavingService { get; set; } = new Mock<IGameSavingService>().Object;
        public ActiveGamesCollection ActiveGamesCollection { get; set; } = new();
        public IGameStartDataSender GameStartDataSender { get; set; } = new Mock<IGameStartDataSender>().Object;
        public IGameCreator GameCreator { get; set; } = GetGameCreatorMock().Object;
        public IGameStartDataCreator GameStartDataCreator { get; set; } = GetGameStartDataCreatorMock().Object;
        
        public GameInitializationController Build()
        {
            return new GameInitializationController(GameSavingService, ActiveGamesCollection, GameStartDataSender, GameCreator, GameStartDataCreator);
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