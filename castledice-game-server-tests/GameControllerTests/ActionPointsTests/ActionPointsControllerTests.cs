using castledice_game_logic.Math;
using castledice_game_logic.TurnsLogic;
using castledice_game_logic.TurnsLogic.TurnSwitchConditions;
using castledice_game_server_tests.TestImplementations;
using castledice_game_server.GameController;
using castledice_game_server.GameController.ActionPoints;
using castledice_game_server.GameController.PlayersReadiness;
using static castledice_game_server_tests.ObjectCreationUtility;
using ILogger = castledice_game_server.Logging.ILogger;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.ActionPointsTests;

public class ActionPointsControllerTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 6)]
    public void GiveActionPointsToCurrentPlayer_ShouldGiveCurrentPlayerNumberOfActionPoints_FromAppropriateGenerator(int currentPlayerId, int numberToGive)
    {
        var gameMock = GetGameMock();
        gameMock.Setup(g => g.GetCurrentPlayer()).Returns(GetPlayer(currentPlayerId));
        var generatorMock = new Mock<IRandomNumberGenerator>();
        generatorMock.Setup(g => g.GetNextRandom()).Returns(numberToGive);
        var generatorsCollection = new Mock<INumberGeneratorsCollection>();
        generatorsCollection.Setup(g => g.GetGeneratorForPlayer(currentPlayerId)).Returns(generatorMock.Object);
        var actionPointsController = new ActionPointsControllerBuilder
        {
            GamesCollection = new TestGamesCollection(),
            NumberGeneratorsCollection = generatorsCollection.Object,
        }.Build();
        
        actionPointsController.GiveActionPointsToCurrentPlayer(gameMock.Object);
        
        gameMock.Verify(g => g.GiveActionPointsToPlayer(currentPlayerId, numberToGive), Times.Once);
    }

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(2, 3, 4, 5)]
    [InlineData(3, 6, 7, 8, 9)]
    public void GiveActionPointsToCurrentPlayer_ShouldSendGivenAmountOfActionPoints_ToAllPlayersFromGame(int amount,
        int actionPointsAccepterId, params int[] otherPlayersIds)
    {
        var gameMock = GetGameMock();
        gameMock.Setup(g => g.GetCurrentPlayer()).Returns(GetPlayer(actionPointsAccepterId));
        var playersIds = new List<int>(otherPlayersIds) {actionPointsAccepterId};
        gameMock.Setup(g => g.GetAllPlayersIds()).Returns(playersIds);
        var generatorMock = new Mock<IRandomNumberGenerator>();
        generatorMock.Setup(g => g.GetNextRandom()).Returns(amount);
        var generatorsCollectionMock = new Mock<INumberGeneratorsCollection>();
        generatorsCollectionMock.Setup(g => g.GetGeneratorForPlayer(actionPointsAccepterId)).Returns(generatorMock.Object);
        var actionPointsSenderMock = new Mock<IActionPointsSender>();
        var actionPointsController = new ActionPointsControllerBuilder
        {
            NumberGeneratorsCollection = generatorsCollectionMock.Object,
            ActionPointsSender = actionPointsSenderMock.Object
        }.Build();
        
        actionPointsController.GiveActionPointsToCurrentPlayer(gameMock.Object);
        
        foreach (var playerId in playersIds)
        {
            actionPointsSenderMock.Verify(a => a.SendActionPoints(amount, actionPointsAccepterId, playerId), Times.Once);
        }
    }

    [Theory]
    [InlineData("Test error message")]
    [InlineData("Some other error message")]
    [InlineData("Yet another error message")]
    public void GiveActionPointsToCurrentPlayer_ShouldLogError_IfExceptionIsThrown(string message)
    {
        var gameMock = GetGameMock();
        gameMock.Setup(p => p.GetCurrentPlayer()).Returns(GetPlayer(1));
        var generatorsCollectionMock = new Mock<INumberGeneratorsCollection>();
        generatorsCollectionMock.Setup(g => g.GetGeneratorForPlayer(It.IsAny<int>())).Throws(new Exception(message));
        var loggerMock = new Mock<ILogger>();
        var actionPointsController = new ActionPointsControllerBuilder
        {
            NumberGeneratorsCollection = generatorsCollectionMock.Object,
            Logger = loggerMock.Object
        }.Build();
        
        actionPointsController.GiveActionPointsToCurrentPlayer(gameMock.Object);
        
        loggerMock.Verify(l => l.Error(message), Times.Once);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(1, 2)]
    [InlineData(1, 2, 3)]
    //As OnGameAdded is private, we test it by using AddGame method of TestGamesCollection
    public void OnGameAdded_ShouldAddActionPointsGenerator_ForEveryPlayerInGame(params int[] playersIds)
    {
        var gameMock = GetGameMock();
        gameMock.Setup(g => g.GetAllPlayersIds()).Returns(new List<int>(playersIds));
        var gamesCollection = new TestGamesCollection();
        var numberGeneratorsMock = new Mock<INumberGeneratorsCollection>();
        var actionPointsController = new ActionPointsControllerBuilder
        {
            GamesCollection = gamesCollection,
            NumberGeneratorsCollection = numberGeneratorsMock.Object
        }.Build();
        
        gamesCollection.AddGame(1, gameMock.Object);
        
        foreach (var playerId in playersIds)
        {
            numberGeneratorsMock.Verify(n => n.AddGeneratorForPlayer(playerId), Times.Once);
        }
    }

    [Fact]
    public void GiveActionPointsToCurrentPlayer_ShouldBeCalled_IfPlayersAreReady()
    {
        var game = GetGameMock().Object;
        var gamesCollection = new TestGamesCollection();
        var generatorMock = new Mock<IRandomNumberGenerator>();
        generatorMock.Setup(g => g.GetNextRandom()).Returns(1);
        var generatorsCollectionMock = new Mock<INumberGeneratorsCollection>();
        generatorsCollectionMock.Setup(g => g.GetGeneratorForPlayer(It.IsAny<int>())).Returns(generatorMock.Object);
        var loggerMock = new Mock<ILogger>();
        var actionPointsSenderMock = new Mock<IActionPointsSender>();
        var notifier = new GamePlayersReadinessNotifier();
        var controllerMock = new Mock<ActionPointsController>(gamesCollection, generatorsCollectionMock.Object,
            actionPointsSenderMock.Object, notifier, loggerMock.Object);
        var testObject = controllerMock.Object; //This line is needed to force constructor call.
        
        notifier.NotifyPlayersAreReady(game);
        
        controllerMock.Verify(c => c.GiveActionPointsToCurrentPlayer(game), Times.Once);
    }

    [Theory]
    [InlineData("Test error message")]
    [InlineData("Some other error message")]
    [InlineData("Yet another error message")]
    public void OnGameAdded_ShouldLogError_IfExceptionIsThrown(string message)
    {
        var gameMock = GetGameMock();
        var gamesCollection = new TestGamesCollection();
        var generatorsCollectionMock = new Mock<INumberGeneratorsCollection>();
        generatorsCollectionMock.Setup(g => g.AddGeneratorForPlayer(It.IsAny<int>())).Throws(new Exception(message));
        var loggerMock = new Mock<ILogger>();
        var controller = new ActionPointsControllerBuilder
        {
            GamesCollection = gamesCollection,
            NumberGeneratorsCollection = generatorsCollectionMock.Object,
            Logger = loggerMock.Object
        }.Build();
        
        gamesCollection.AddGame(1, gameMock.Object);
        
        loggerMock.Verify(l => l.Error(message), Times.Once);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(1, 2)]
    [InlineData(1, 2, 3)]
    public void OnGameRemoved_ShouldRemoveActionPointsGenerator_ForEveryPlayerInGame(params int[] playersIds)
    {
        var gameMock = GetGameMock();
        gameMock.Setup(g => g.GetAllPlayersIds()).Returns(new List<int>(playersIds));
        var gamesCollection = new TestGamesCollection();
        gamesCollection.GameToReturnOnGameRemoved = gameMock.Object;
        var numberGeneratorsMock = new Mock<INumberGeneratorsCollection>();
        var actionPointsController = new ActionPointsControllerBuilder
        {
            GamesCollection = gamesCollection,
            NumberGeneratorsCollection = numberGeneratorsMock.Object
        }.Build();
        
        gamesCollection.RemoveGame(1);
        
        foreach (var playerId in playersIds)
        {
            numberGeneratorsMock.Verify(n => n.RemoveGeneratorForPlayer(playerId), Times.Once);
        }
    }
    
    [Fact]
    public void OnTurnSwitched_ShouldCallGiveActionPointsToCurrentPlayer_WithGivenGame()
    {
        var expectedGame = GetGameMock().Object;
        var gamesCollection = new TestGamesCollection();
        var generatorMock = new Mock<IRandomNumberGenerator>();
        generatorMock.Setup(g => g.GetNextRandom()).Returns(1);
        var generatorsCollectionMock = new Mock<INumberGeneratorsCollection>();
        generatorsCollectionMock.Setup(g => g.GetGeneratorForPlayer(It.IsAny<int>())).Returns(generatorMock.Object);
        var loggerMock = new Mock<ILogger>();
        var actionPointsSenderMock = new Mock<IActionPointsSender>();
        var notifier = new GamePlayersReadinessNotifier();
        var controllerMock = new Mock<ActionPointsController>(gamesCollection, generatorsCollectionMock.Object,
            actionPointsSenderMock.Object, notifier, loggerMock.Object);
        var testObject = controllerMock.Object; //This line is needed to force constructor call.
        
        gamesCollection.AddGame(1, expectedGame);
        expectedGame.SwitchTurn(); //Forcing turn switch in game
        
        controllerMock.Verify(c => c.GiveActionPointsToCurrentPlayer(expectedGame), Times.Exactly(1));
    }

    [Fact]
    public void OnTurnSwitched_ShouldNotCallGiveActionPointsToCurrentPlayer_IfGameWasRemoved()
    {
        var expectedGame = GetGameMock().Object;
        var gamesCollection = new TestGamesCollection();
        gamesCollection.GameToReturnOnGameRemoved = expectedGame;
        var generatorMock = new Mock<IRandomNumberGenerator>();
        generatorMock.Setup(g => g.GetNextRandom()).Returns(1);
        var generatorsCollectionMock = new Mock<INumberGeneratorsCollection>();
        generatorsCollectionMock.Setup(g => g.GetGeneratorForPlayer(It.IsAny<int>())).Returns(generatorMock.Object);
        var loggerMock = new Mock<ILogger>();
        var actionPointsSenderMock = new Mock<IActionPointsSender>();
        var notifier = new GamePlayersReadinessNotifier();
        var controllerMock = new Mock<ActionPointsController>(gamesCollection, generatorsCollectionMock.Object,
            actionPointsSenderMock.Object, notifier, loggerMock.Object);
        var testObject = controllerMock.Object; //This line is needed to force constructor call.
        
        gamesCollection.AddGame(1, expectedGame);
        gamesCollection.RemoveGame(1);
        expectedGame.SwitchTurn(); //Forcing turn switch in game
        
        controllerMock.Verify(c => c.GiveActionPointsToCurrentPlayer(expectedGame), Times.Never); 
    }

    private class ActionPointsControllerBuilder
    {
        public IGamesCollection GamesCollection { get; set; } = new Mock<IGamesCollection>().Object;
        public INumberGeneratorsCollection NumberGeneratorsCollection { get; set; } = GetGeneratorsCollectionMock().Object;
        public IActionPointsSender ActionPointsSender { get; set; } = new Mock<IActionPointsSender>().Object;
        public IGamePlayersReadinessNotifier PlayersReadinessNotifier { get; set; } = new GamePlayersReadinessNotifier();
        public ILogger Logger { get; set; } = new Mock<ILogger>().Object;
        
        public ActionPointsController Build()
        {
            return new(GamesCollection, NumberGeneratorsCollection, ActionPointsSender, PlayersReadinessNotifier,  Logger);
        }
    }

    private static Mock<INumberGeneratorsCollection> GetGeneratorsCollectionMock()
    {
        var mock = new Mock<INumberGeneratorsCollection>();
        var generatorMock = new Mock<IRandomNumberGenerator>();
        generatorMock.Setup(g => g.GetNextRandom()).Returns(1);
        mock.Setup(m => m.GetGeneratorForPlayer(It.IsAny<int>())).Returns(generatorMock.Object);
        return mock;
    }
}