using castledice_game_server.GameController;
using castledice_game_server.GameController.General;
using castledice_game_server.GameController.PlayersDisconnectHandling;
using castledice_game_server.NetworkManager.PlayersTracking;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.PlayerDisconnectHandlingTests;

public class PlayerDisconnectControllerTests
{
    private readonly Random _random = new();
    
    [Fact]
    public void Controller_ShouldRemovePair_ByPlayerId_FromIdsMap_WhenPlayerDisconnects()
    {
        var idsMapMock = new Mock<IPlayerToClientIdsMap>();
        var eventEmitterMock = new Mock<IPlayerDisconnectedEventEmitter>();
        var playerId = _random.Next();
        var controller = new ControllerBuilder { IdsMap = idsMapMock.Object, EventEmitter = eventEmitterMock.Object}.Build();
        
        eventEmitterMock.Raise(x => x.PlayerDisconnected += null, playerId);
        
        idsMapMock.Verify(x => x.RemovePairByPlayer(playerId), Times.Once);
    }

    [Fact]
    public void Controller_ShouldRemoveGame_FromGamesCollection_IfPlayerWasInGame()
    {
        var gamesCollectionMock = new Mock<IGamesCollection>();
        var eventEmitterMock = new Mock<IPlayerDisconnectedEventEmitter>();
        var playerId = _random.Next();
        var game = GetGame();
        var gameForPlayerProviderMock = new Mock<IGameForPlayerProvider>();
        gameForPlayerProviderMock.Setup(x => x.PlayerIsInGame(playerId)).Returns(true);
        gameForPlayerProviderMock.Setup(x => x.GetGame(playerId)).Returns(game);
        var controller = new ControllerBuilder
        {
            GamesCollection = gamesCollectionMock.Object, 
            EventEmitter = eventEmitterMock.Object, 
            GameForPlayerProvider = gameForPlayerProviderMock.Object
        }.Build();
        
        eventEmitterMock.Raise(x => x.PlayerDisconnected += null, playerId);
        
        gamesCollectionMock.Verify(x => x.RemoveGame(game), Times.Once);
    }

    [Fact]
    public void Controller_ShouldSendPlayerDisconnected_WithDisconnectedPlayerId_ToOtherPlayersOfGame_IfDisconnectedPlayerWasInGame()
    {
        var disconnectedSenderMock = new Mock<IPlayerDisconnectedSender>();
        var otherPlayerId = _random.Next();
        var playerId = _random.Next();
        var game = GetGame(GetPlayer(playerId), GetPlayer(otherPlayerId));
        var gameForPlayerProviderMock = new Mock<IGameForPlayerProvider>();
        gameForPlayerProviderMock.Setup(x => x.PlayerIsInGame(playerId)).Returns(true);
        gameForPlayerProviderMock.Setup(x => x.GetGame(playerId)).Returns(game);
        var eventEmitterMock = new Mock<IPlayerDisconnectedEventEmitter>();
        var controller = new ControllerBuilder
        {
            DisconnectedSender = disconnectedSenderMock.Object,
            EventEmitter = eventEmitterMock.Object,
            GameForPlayerProvider = gameForPlayerProviderMock.Object
        }.Build();
        
        eventEmitterMock.Raise(x => x.PlayerDisconnected += null, playerId);
        
        disconnectedSenderMock.Verify(x => x.SendPlayerDisconnectedMessage(playerId, otherPlayerId), Times.Once);
    }

    private class ControllerBuilder
    {
        public IPlayerDisconnectedEventEmitter EventEmitter { get; set; }
        public IPlayerToClientIdsMap IdsMap { get; set; }
        public IGamesCollection GamesCollection { get; set; }
        public IGameForPlayerProvider GameForPlayerProvider { get; set; }
        public IPlayerDisconnectedSender DisconnectedSender { get; set; }

        public ControllerBuilder()
        {
            var eventEmitterMock = new Mock<IPlayerDisconnectedEventEmitter>();
            EventEmitter = eventEmitterMock.Object;
            var idsMapMock = new Mock<IPlayerToClientIdsMap>();
            IdsMap = idsMapMock.Object;
            var gamesCollectionMock = new Mock<IGamesCollection>();
            GamesCollection = gamesCollectionMock.Object;
            var gameForPlayerProviderMock = new Mock<IGameForPlayerProvider>();
            gameForPlayerProviderMock.Setup(x => x.PlayerIsInGame(It.IsAny<int>())).Returns(false);
            GameForPlayerProvider = gameForPlayerProviderMock.Object;
            var disconnectedSenderMock = new Mock<IPlayerDisconnectedSender>();
            DisconnectedSender = disconnectedSenderMock.Object;
        }
        
        public PlayerDisconnectController Build()
        {
            return new PlayerDisconnectController(EventEmitter, IdsMap, GamesCollection, GameForPlayerProvider, DisconnectedSender);
        }
    }
}