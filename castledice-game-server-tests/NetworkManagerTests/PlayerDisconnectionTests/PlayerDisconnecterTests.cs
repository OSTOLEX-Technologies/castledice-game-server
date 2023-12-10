using castledice_game_server.NetworkManager.PlayerDisconnection;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using Moq;

namespace castledice_game_server_tests.NetworkManagerTests.PlayerDisconnectionTests;

public class PlayerDisconnecterTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void DisconnectPlayerWithId_ShouldPassGivenId_ToPlayerClientIdProvider(int id)
    {
        var removerMock = new Mock<IPlayerClientIdRemover>();
        var clientDisconnecterMock = new Mock<IClientDisconnecter>();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        var disconnecter = new PlayerDisconnecter(clientDisconnecterMock.Object, removerMock.Object, clientIdProviderMock.Object);
        
        disconnecter.DisconnectPlayerWithId(id);
        
        clientIdProviderMock.Verify(provider => provider.GetClientIdForPlayer(id), Times.Once);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 4)]
    [InlineData(4, 5)]
    public void DisconnectPlayerWithId_ShouldPassClientIdFromProvider_ToClientDisconnecter(int playerId, ushort clientId)
    {
        var removerMock = new Mock<IPlayerClientIdRemover>();
        var clientDisconnecterMock = new Mock<IClientDisconnecter>();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        clientIdProviderMock.Setup(provider => provider.GetClientIdForPlayer(playerId)).Returns(clientId);
        var playerDisconnecter = new PlayerDisconnecter(clientDisconnecterMock.Object, removerMock.Object, clientIdProviderMock.Object);
        
        playerDisconnecter.DisconnectPlayerWithId(playerId);
        
        clientDisconnecterMock.Verify(disconnecter => disconnecter.DisconnectClient(clientId, null), Times.Once);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void DisconnectPlayerWithId_ShouldCallRemoveClientIdForPlayer_OnGivenPlayerClientIdRemover(int id)
    {
        var removerMock = new Mock<IPlayerClientIdRemover>();
        var clientDisconnecterMock = new Mock<IClientDisconnecter>();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        var playerDisconnecter = new PlayerDisconnecter(clientDisconnecterMock.Object, removerMock.Object, clientIdProviderMock.Object);
        
        playerDisconnecter.DisconnectPlayerWithId(id);
        
        removerMock.Verify(remover => remover.RemoveClientIdForPlayer(id), Times.Once);
    }
}