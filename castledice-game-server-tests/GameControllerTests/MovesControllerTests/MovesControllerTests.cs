using castledice_game_data_logic.MoveConverters;
using castledice_game_data_logic.Moves;
using castledice_game_logic;
using castledice_game_logic.GameObjects;
using castledice_game_logic.Math;
using castledice_game_logic.MovesLogic;
using castledice_game_server.Exceptions;
using castledice_game_server.GameController.Moves;
using static castledice_game_server_tests.ObjectCreationUtility;
using ILogger = castledice_game_server.Logging.ILogger;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.MovesControllerTests;

public class MovesControllerTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void MakeMove_ShouldPassPlayerIdFromGivenMoveData_ToGivenGameForPlayerProvider(int playerId)
    {
        var gameForPlayerProviderMock = GetGameProviderMock();
        var movesController = new MovesControllerBuilder { GameProvider = gameForPlayerProviderMock.Object }.Build();
        var moveData = GetMoveData(playerId);
        
        movesController.MakeMove(moveData);
        
        gameForPlayerProviderMock.Verify(g => g.GetGame(playerId), Times.Once);
    }

    [Theory]
    [InlineData("some error")]
    [InlineData("some other error")]
    [InlineData("yet another error")]
    public void MakeMove_ShouldLogExceptionMessage_IfGivenGameForPlayerProviderWillThrowOne(string message)
    {
        var exceptionToThrow = new GameNotFoundException(message);
        var gameForPlayerProviderMock = GetGameProviderMock();
        gameForPlayerProviderMock.Setup(g => g.GetGame(It.IsAny<int>())).Throws(exceptionToThrow);
        var loggerMock = new Mock<ILogger>();
        var movesController = new MovesControllerBuilder { GameProvider = gameForPlayerProviderMock.Object, Logger = loggerMock.Object }.Build();
        var moveData = GetMoveData();
        
        movesController.MakeMove(moveData);
        
        loggerMock.Verify(l => l.Error(exceptionToThrow.Message), Times.Once);
    }

    [Fact]
    public void MakeMove_ShouldPassFoundGame_ToGivenConverterProvider()
    {
        var game = GetGameMock().Object;
        var gameForPlayerProviderMock = GetGameProviderMock();
        gameForPlayerProviderMock.Setup(g => g.GetGame(It.IsAny<int>())).Returns(game);
        var converterProviderMock = GetDataConverterProviderMock();
        var movesController = new MovesControllerBuilder { GameProvider = gameForPlayerProviderMock.Object, DataToMoveConverterProvider = converterProviderMock.Object }.Build();
        
        movesController.MakeMove(GetMoveData());
        
        converterProviderMock.Verify(c => c.GetDataToMoveConverter(game), Times.Once);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void MakeMove_ShouldPassPassMoveDataAndPlayerFromFoundGame_ToConverterFromGivenProvider(int playerId)
    {
        var player = GetPlayer(playerId);
        var gameMock = GetGameMock();
        gameMock.Setup(g => g.GetPlayer(playerId)).Returns(player);
        var gameForPlayerProviderMock = GetGameProviderMock();
        gameForPlayerProviderMock.Setup(g => g.GetGame(It.IsAny<int>())).Returns(gameMock.Object);
        var converterProviderMock = GetDataConverterProviderMock();
        var converterMock = GetConverterMock();
        converterProviderMock.Setup(c => c.GetDataToMoveConverter(gameMock.Object)).Returns(converterMock.Object);
        var movesController = new MovesControllerBuilder { GameProvider = gameForPlayerProviderMock.Object, DataToMoveConverterProvider = converterProviderMock.Object }.Build();
        var moveData = GetMoveData(playerId);
        
        movesController.MakeMove(moveData);
        
        converterMock.Verify(c => c.ConvertToMove(moveData, player), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void MakeMove_ShouldSendFalseMoveApprovalToPlayer_IfMoveIsNotApplied(int playerId)
    {
        var gameMock = GetGameMock();
        gameMock.Setup(g => g.TryMakeMove(It.IsAny<AbstractMove>())).Returns(false);
        var gameForPlayerProviderMock = GetGameProviderMock();
        gameForPlayerProviderMock.Setup(g => g.GetGame(It.IsAny<int>())).Returns(gameMock.Object);
        var moveStatusSenderMock = new Mock<IMoveStatusSender>();
        var moveData = GetMoveData(playerId);
        var movesController = new MovesControllerBuilder { GameProvider = gameForPlayerProviderMock.Object, MoveStatusSender = moveStatusSenderMock.Object }.Build();
        
        movesController.MakeMove(moveData);
        
        moveStatusSenderMock.Verify(m => m.SendMoveStatusToPlayer(false, playerId), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void MakeMove_ShouldSendTrueMoveApprovalToPlayer_IfMoveIsApplied(int playerId)
    {
        var gameMock = GetGameMock();
        gameMock.Setup(g => g.TryMakeMove(It.IsAny<AbstractMove>())).Returns(true);
        var gameForPlayerProviderMock = GetGameProviderMock();
        gameForPlayerProviderMock.Setup(g => g.GetGame(It.IsAny<int>())).Returns(gameMock.Object);
        var moveStatusSenderMock = new Mock<IMoveStatusSender>();
        var moveData = GetMoveData(playerId);
        var movesController = new MovesControllerBuilder { GameProvider = gameForPlayerProviderMock.Object, MoveStatusSender = moveStatusSenderMock.Object }.Build();
        
        movesController.MakeMove(moveData);
        
        moveStatusSenderMock.Verify(m => m.SendMoveStatusToPlayer(true, playerId), Times.Once);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(3, 1, 2)]
    [InlineData(4, 1, 2, 3)]
    public void MakeMove_ShouldSendMoveDataToOtherPlayersOfGame_IfMoveIsApplied(int playerId,
        params int[] otherPlayersIds)
    {
        var playersIdsList = new List<int>(otherPlayersIds);
        playersIdsList.Add(playerId);
        var gameMock = GetGameMock();
        gameMock.Setup(g => g.TryMakeMove(It.IsAny<AbstractMove>())).Returns(true);
        gameMock.Setup(g => g.GetAllPlayersIds()).Returns(playersIdsList);
        var gameForPlayerProviderMock = GetGameProviderMock();
        gameForPlayerProviderMock.Setup(g => g.GetGame(It.IsAny<int>())).Returns(gameMock.Object);
        var moveDataSenderMock = new Mock<IMoveDataSender>();
        var moveData = GetMoveData(playerId);
        var movesController = new MovesControllerBuilder { GameProvider = gameForPlayerProviderMock.Object, MoveDataSender = moveDataSenderMock.Object }.Build();
        
        movesController.MakeMove(moveData);

        foreach (var id in otherPlayersIds)
        {
            moveDataSenderMock.Verify(m => m.SendDataToPlayer(moveData, id), Times.Once);
        }
    }

    private static MoveData GetMoveData(int playerId = 1)
    {
        return GetMoveData((0, 0), playerId);
    }
    
    private static MoveData GetMoveData(Vector2Int position, int playerId = 1)
    {
        var mock = new Mock<MoveData>(playerId, position);
        return mock.Object;
    }
    
    private class MovesControllerBuilder
    {
        public IGameForPlayerProvider GameProvider { get; set; } = GetGameProviderMock().Object;
        public IDataToMoveConverterProvider DataToMoveConverterProvider { get; set; } = GetDataConverterProviderMock().Object;
        public IMoveDataSender MoveDataSender { get; set; } = new Mock<IMoveDataSender>().Object;
        public IMoveStatusSender MoveStatusSender { get; set; } = new Mock<IMoveStatusSender>().Object;
        public ILogger Logger { get; set; } = new Mock<ILogger>().Object;
        
        public MovesController Build()
        {
            return new MovesController(GameProvider, DataToMoveConverterProvider, MoveDataSender, MoveStatusSender, Logger);
        }
    }

    private static Mock<IGameForPlayerProvider> GetGameProviderMock()
    {
        var mock = new Mock<IGameForPlayerProvider>();
        var gameMock = GetGameMock();
        mock.Setup(x => x.GetGame(It.IsAny<int>())).Returns(gameMock.Object);
        return mock;
    }

    private static Mock<IDataToMoveConverterProvider> GetDataConverterProviderMock()
    {
        var mock = new Mock<IDataToMoveConverterProvider>();
        var converterMock = GetConverterMock();
        mock.Setup(x => x.GetDataToMoveConverter(It.IsAny<Game>())).Returns(converterMock.Object);
        return mock;
    }

    private static Mock<IDataToMoveConverter> GetConverterMock()
    {
        var converterMock = new Mock<IDataToMoveConverter>();
        converterMock.Setup(x => x.ConvertToMove(It.IsAny<MoveData>(), It.IsAny<Player>())).Returns(new Mock<AbstractMove>(GetPlayer(1), new Vector2Int(1, 1)).Object);
        return converterMock;
    }
}