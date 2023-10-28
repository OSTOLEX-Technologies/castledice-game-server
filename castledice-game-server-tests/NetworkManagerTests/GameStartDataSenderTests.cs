using casltedice_events_logic.ServerToClient;
using castledice_game_server_tests.TestImplementations;
using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_riptide_dto_adapters.Extensions;
using Moq;
using Riptide;
using static castledice_game_server_tests.ObjectCreationUtility;

namespace castledice_game_server_tests;

public class GameStartDataSenderTests
{
    
    [Theory]
    [MemberData(nameof(PlayersToClientsIds))]
    public void SendGameStartData_ShouldSendMessageToPlayersFromGameStartData_WithClientIdsFromGivenProvider(Dictionary<int, ushort> playersToClientsIds)
    {
        var messageSenderMock = new Mock<IMessageSenderById>();
        var gameStartData = GetGameStartData(playersToClientsIds.Keys.ToArray());
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        foreach (var playersToClientsId in playersToClientsIds)
        {
            clientIdProviderMock.Setup(provider => provider.GetClientIdForPlayer(playersToClientsId.Key)).Returns(playersToClientsId.Value);
        }
        var gameStartDataSender = new GameStartDataSender(messageSenderMock.Object, clientIdProviderMock.Object);
        
        gameStartDataSender.SendGameStartData(gameStartData);

        foreach (var clientId in playersToClientsIds.Values)
        {
            messageSenderMock.Verify(s => s.Send(It.IsAny<Message>(), clientId), Times.Once);
        }
    }

    public static IEnumerable<object[]> PlayersToClientsIds()
    {
        yield return new[]
        {
            new Dictionary<int, ushort>
            {
                { 1, 3 },
                { 2, 4 }
            }
        };
        yield return new[]
        {
            new Dictionary<int, ushort>
            {
                { 1, 3 },
                { 2, 4 },
                { 3, 5 }
            }
        };
    }

    [Fact]
    public void SendGameStartData_ShouldSendMessage_WithAppropriateCreateGameDTO()
    {
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var gameStartData = GetGameStartData(firstPlayerId, secondPlayerId);
        var messageSender = new TestMessageSenderById();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        clientIdProviderMock.Setup(provider => provider.GetClientIdForPlayer(firstPlayerId)).Returns(3);
        clientIdProviderMock.Setup(provider => provider.GetClientIdForPlayer(secondPlayerId)).Returns(4);
        var gameStartDataSender = new GameStartDataSender(messageSender, clientIdProviderMock.Object);
        var expectedDTO = new CreateGameDTO(gameStartData);
        
        gameStartDataSender.SendGameStartData(gameStartData);
        var sentMessage = messageSender.SentMessage;
        sentMessage.GetByte();
        sentMessage.GetByte();
        var sentDTO = sentMessage.GetCreateGameDTO();

        Assert.Equal(expectedDTO, sentDTO);
    }
}