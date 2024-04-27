using castledice_game_server.NetworkManager.PlayerDisconnection;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using Moq;

namespace castledice_game_server_tests.NetworkManagerTests.PlayerDisconnectionTests;

public class PlayerDisconnecterTests
{
   [Fact]
   public void DisconnectPlayerWithId_ShouldCallDisconnectClient_ForClientIdFromIdsMap()
   {
      var clientDisconnecterMock = new Mock<IClientDisconnecter>();
      var idsMapMock = new Mock<IPlayerToClientIdsMap>();
      var random = new Random();
      var playerId = random.Next();
      var clientId = (ushort)random.Next();
      idsMapMock.Setup(map => map.GetClientByPlayer(playerId)).Returns(clientId);
      var playerDisconnecter = new PlayerDisconnecter(clientDisconnecterMock.Object, idsMapMock.Object);
      
      playerDisconnecter.DisconnectPlayerWithId(playerId);

      clientDisconnecterMock.Verify(disconnecter => disconnecter.DisconnectClient(clientId, null), Times.Once);
   }
}