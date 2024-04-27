using castledice_events_logic.ServerToClient;
using castledice_game_server.Auth;
using castledice_game_server.GameController.PlayerInitialization;
using castledice_game_server.Logging;
using castledice_game_server.NetworkManager.PlayerDisconnection;
using castledice_game_server.NetworkManager.PlayersTracking;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.PlayerInitializationTests;

public class PlayerInitializationControllerTests
{
    [Theory]
    [InlineData("token")]
    [InlineData("anotherToken")]
    [InlineData("yetAnotherToken")]
    public async void InitializePlayerAsync_ShouldPassGiven_ToGivenIdRetriever(string token)
    {
        var retrieverMock = new Mock<IIdRetriever>();
        var initializer = new PlayerInitializerBuilder
        {
            IdRetriever = retrieverMock.Object
        }.Build();
        
        await initializer.InitializePlayerAsync(token, 0);
        
        retrieverMock.Verify(retriever => retriever.RetrievePlayerIdAsync(token), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void InitializePlayerAsync_ShouldDisconnectPlayerById_IfPlayerHasClientId(int playerId)
    {
        var retrieverMock = new Mock<IIdRetriever>();
        retrieverMock.Setup(retriever => retriever.RetrievePlayerIdAsync(It.IsAny<string>())).ReturnsAsync(playerId);
        var disconnecterMock = new Mock<IPlayerDisconnecter>();
        var clientIdProviderMock = new Mock<IPlayerToClientIdsMap>();
        clientIdProviderMock.Setup(c => c.PlayerHasClient(playerId)).Returns(true);
        var initializer = new PlayerInitializerBuilder
        {
            IdRetriever = retrieverMock.Object,
            PlayerDisconnecter = disconnecterMock.Object,
            IdsMap = clientIdProviderMock.Object
        }.Build();
        
        await initializer.InitializePlayerAsync("token", 0);
        
        disconnecterMock.Verify(disconnecter => disconnecter.DisconnectPlayerWithId(playerId), Times.Once);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void InitializePlayerAsync_ShouldNotDisconnectPlayerById_IfPlayerDoesNotHaveClientId(int playerId)
    {
        var retrieverMock = new Mock<IIdRetriever>();
        retrieverMock.Setup(retriever => retriever.RetrievePlayerIdAsync(It.IsAny<string>())).ReturnsAsync(playerId);
        var disconnecterMock = new Mock<IPlayerDisconnecter>();
        var clientIdProviderMock = new Mock<IPlayerToClientIdsMap>();
        clientIdProviderMock.Setup(c => c.PlayerHasClient(playerId)).Returns(false);
        var initializer = new PlayerInitializerBuilder
        {
            IdRetriever = retrieverMock.Object,
            PlayerDisconnecter = disconnecterMock.Object,
            IdsMap = clientIdProviderMock.Object
        }.Build();
        
        await initializer.InitializePlayerAsync("token", 0);
        
        disconnecterMock.Verify(disconnecter => disconnecter.DisconnectPlayerWithId(playerId), Times.Never);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 4)]
    public async void InitializePlayerAsync_ShouldSaveClientId_ByUsingGivenPlayerClientIdSaver(int playerId, ushort clientId)
    {
        var retrieverMock = new Mock<IIdRetriever>();
        retrieverMock.Setup(retriever => retriever.RetrievePlayerIdAsync(It.IsAny<string>())).ReturnsAsync(playerId);
        var clientIdSaverMock = new Mock<IPlayerToClientIdsMap>();
        var initializer = new PlayerInitializerBuilder
        {
            IdRetriever = retrieverMock.Object,
            IdsMap = clientIdSaverMock.Object
        }.Build();
        
        await initializer.InitializePlayerAsync("token", clientId);
        
        clientIdSaverMock.Verify(saver => saver.SaveClientToPlayer(playerId, clientId), Times.Once);
    }

    [Fact]
    public async void InitializePlayerAsync_ShouldLogThrownExceptions()
    {
        var retrieverMock = new Mock<IIdRetriever>();
        var expectedException = new Exception();
        retrieverMock.Setup(retriever => retriever.RetrievePlayerIdAsync(It.IsAny<string>())).Throws(expectedException);
        var loggerMock = new Mock<ILogger>();
        var initializer = new PlayerInitializerBuilder
        {
            IdRetriever = retrieverMock.Object,
            Logger = loggerMock.Object
        }.Build();
        
        await initializer.InitializePlayerAsync("token", 0);
        
        loggerMock.Verify(logger => logger.Error(expectedException), Times.Once);
    }

    [Fact]
    public async void InitializePlayerAsync_ShouldSendTrueInitializationResultToClient_IfNoErrorsOccured()
    {
        var expectedClientId = (ushort) new Random().Next(ushort.MinValue, ushort.MaxValue);
        var initializationResultSenderMock = new Mock<IPlayerInitializationResultDTOSender>();
        var initializer = new PlayerInitializerBuilder
        {
            PlayerInitializationResultDtoSender = initializationResultSenderMock.Object
        }.Build();
        
        await initializer.InitializePlayerAsync("token", expectedClientId);
        
        initializationResultSenderMock.Verify(sender => sender.SendInitializationResult(expectedClientId, new PlayerInitializationResultDTO(true)), Times.Once);
    }
    
    [Fact]
    public async void InitializePlayerAsync_ShouldSendFalseInitializationResultToClient_IfErrorOccured()
    {
        var expectedClientId = (ushort) new Random().Next(ushort.MinValue, ushort.MaxValue);
        var initializationResultSenderMock = new Mock<IPlayerInitializationResultDTOSender>();
        var retrieverMock = new Mock<IIdRetriever>();
        retrieverMock.Setup(retriever => retriever.RetrievePlayerIdAsync(It.IsAny<string>())).Throws(new Exception());
        var initializer = new PlayerInitializerBuilder
        {
            PlayerInitializationResultDtoSender = initializationResultSenderMock.Object,
            IdRetriever = retrieverMock.Object
        }.Build();
        
        await initializer.InitializePlayerAsync("token", expectedClientId);
        
        initializationResultSenderMock.Verify(sender => sender.SendInitializationResult(expectedClientId, new PlayerInitializationResultDTO(false)), Times.Once);
    }

    private class PlayerInitializerBuilder
    {
        public IIdRetriever IdRetriever { get; set; } = new Mock<IIdRetriever>().Object;
        public IPlayerToClientIdsMap IdsMap { get; set; } = new Mock<IPlayerToClientIdsMap>().Object;
        public IPlayerDisconnecter PlayerDisconnecter { get; set; } = new Mock<IPlayerDisconnecter>().Object;
        public IPlayerInitializationResultDTOSender PlayerInitializationResultDtoSender { get; set; } = new Mock<IPlayerInitializationResultDTOSender>().Object;
        public ILogger Logger { get; set; } = new Mock<ILogger>().Object;
        
        public PlayerInitializationController Build()
        {
            return new PlayerInitializationController(IdRetriever, IdsMap, PlayerDisconnecter, PlayerInitializationResultDtoSender, Logger);
        }
    }

}