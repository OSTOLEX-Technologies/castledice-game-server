using casltedice_events_logic.ClientToServer;
using castledice_game_server.Auth;
using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.PlayerDisconnection;
using castledice_game_server.NetworkManager.PlayersTracking;
using Moq;

namespace castledice_game_server_tests.NetworkManagerTests;

public class PlayerInitializerTests
{
    [Theory]
    [InlineData("token")]
    [InlineData("anotherToken")]
    [InlineData("yetAnotherToken")]
    public void InitializePlayer_ShouldPassTokenFromDTO_ToGivenIdRetriever(string token)
    {
        var retrieverMock = new Mock<IIdRetriever>();
        var initializer = new PlayerInitializerBuilder
        {
            IdRetriever = retrieverMock.Object
        }.Build();
        
        initializer.InitializePlayer(new InitializePlayerDTO(token), 0);
        
        retrieverMock.Verify(retriever => retriever.RetrievePlayerId(token), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void InitializePlayer_ShouldDisconnectPlayerById_IfPlayerHasClientId(int playerId)
    {
        var retrieverMock = new Mock<IIdRetriever>();
        retrieverMock.Setup(retriever => retriever.RetrievePlayerId(It.IsAny<string>())).Returns(playerId);
        var disconnecterMock = new Mock<IPlayerDisconnecter>();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        clientIdProviderMock.Setup(c => c.PlayerHasClientId(playerId)).Returns(true);
        var initializer = new PlayerInitializerBuilder
        {
            IdRetriever = retrieverMock.Object,
            PlayerDisconnecter = disconnecterMock.Object,
            PlayerClientIdProvider = clientIdProviderMock.Object
        }.Build();
        
        initializer.InitializePlayer(new InitializePlayerDTO("token"), 0);
        
        disconnecterMock.Verify(disconnecter => disconnecter.DisconnectPlayerWithId(playerId), Times.Once);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void InitializePlayer_ShouldNotDisconnectPlayerById_IfPlayerDoesNotHaveClientId(int playerId)
    {
        var retrieverMock = new Mock<IIdRetriever>();
        retrieverMock.Setup(retriever => retriever.RetrievePlayerId(It.IsAny<string>())).Returns(playerId);
        var disconnecterMock = new Mock<IPlayerDisconnecter>();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        clientIdProviderMock.Setup(c => c.PlayerHasClientId(playerId)).Returns(false);
        var initializer = new PlayerInitializerBuilder
        {
            IdRetriever = retrieverMock.Object,
            PlayerDisconnecter = disconnecterMock.Object,
            PlayerClientIdProvider = clientIdProviderMock.Object
        }.Build();
        
        initializer.InitializePlayer(new InitializePlayerDTO("token"), 0);
        
        disconnecterMock.Verify(disconnecter => disconnecter.DisconnectPlayerWithId(playerId), Times.Never);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 4)]
    public void InitializePlayer_ShouldSaveClientId_ByUsingGivenPlayerClientIdSaver(int playerId, ushort clientId)
    {
        var retrieverMock = new Mock<IIdRetriever>();
        retrieverMock.Setup(retriever => retriever.RetrievePlayerId(It.IsAny<string>())).Returns(playerId);
        var clientIdSaverMock = new Mock<IPlayerClientIdSaver>();
        var initializer = new PlayerInitializerBuilder
        {
            IdRetriever = retrieverMock.Object,
            PlayerClientIdSaver = clientIdSaverMock.Object
        }.Build();
        
        initializer.InitializePlayer(new InitializePlayerDTO("token"), clientId);
        
        clientIdSaverMock.Verify(saver => saver.SaveClientIdForPlayer(playerId, clientId), Times.Once);
    }

    private class PlayerInitializerBuilder
    {
        public IIdRetriever IdRetriever { get; set; } = new Mock<IIdRetriever>().Object;
        public IPlayerClientIdSaver PlayerClientIdSaver { get; set; } = new Mock<IPlayerClientIdSaver>().Object;
        public IPlayerClientIdProvider PlayerClientIdProvider { get; set; } = new Mock<IPlayerClientIdProvider>().Object;
        public IPlayerDisconnecter PlayerDisconnecter { get; set; } = new Mock<IPlayerDisconnecter>().Object;
        
        public PlayerInitializer Build()
        {
            return new PlayerInitializer(IdRetriever, PlayerClientIdSaver, PlayerClientIdProvider, PlayerDisconnecter);
        }
    }

}