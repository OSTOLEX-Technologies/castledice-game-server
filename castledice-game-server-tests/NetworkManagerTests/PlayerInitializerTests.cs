using casltedice_events_logic.ClientToServer;
using castledice_game_server.Auth;
using castledice_game_server.NetworkManager;
using Moq;

namespace castledice_game_server_tests;

public class PlayerInitializerTests
{
    [Fact]
    public void InitializePlayer_ShouldAddPlayerAndClientIdsToDictionary()
    {
        var idRetriever = new Mock<IIdRetriever>();
        var dto = new InitializePlayerDTO("token");
        var clientId = (ushort) 1;
        var playerId = 3;
        idRetriever.Setup(x => x.RetrievePlayerId(dto.VerificationKey)).Returns(playerId);
        var playerInitializer = new PlayerInitializer(idRetriever.Object);
        
        playerInitializer.InitializePlayer(dto, clientId);
        
        Assert.Equal(clientId, PlayersDictionary.Dictionary[playerId]);
        PlayersDictionary.Dictionary.Clear();
    }

    [Fact]
    public void InitializePlayer_ShouldRemoveOldClientIdAndAddNew_IfGivenPlayerIdAlreadyInDictionary()
    {
        var oldClientId = (ushort) 3;
        var newClientId = (ushort) 5;
        var playerId = 3;
        PlayersDictionary.Dictionary.Add(playerId, oldClientId);
        var idRetriever = new Mock<IIdRetriever>();
        var dto = new InitializePlayerDTO("token");
        idRetriever.Setup(x => x.RetrievePlayerId(dto.VerificationKey)).Returns(playerId);
        var playerInitializer = new PlayerInitializer(idRetriever.Object);
        
        playerInitializer.InitializePlayer(dto, newClientId);
        
        Assert.Equal(newClientId, PlayersDictionary.Dictionary[playerId]);
        PlayersDictionary.Dictionary.Clear();
    }
}