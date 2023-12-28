using castledice_events_logic.ServerToClient;
using castledice_game_data_logic.Moves;
using castledice_game_logic.Math;
using castledice_game_server_tests.TestImplementations;
using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Moq;
using Riptide;

namespace castledice_game_server_tests.NetworkManagerTests;

public class MoveDataSenderTests
{
    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 5)]
    [InlineData(13, 213)]
    public void SendDataToPlayer_ShouldSendMessage_ToAppropriateClientId(int playerId, ushort clientId)
    {
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        clientIdProviderMock.Setup(provider => provider.GetClientIdForPlayer(playerId)).Returns(clientId);
        var messageSenderMock = new Mock<IMessageSenderById>();
        var moveDataSender = new MoveDataSender(messageSenderMock.Object, clientIdProviderMock.Object);
        
        moveDataSender.SendDataToPlayer(new Mock<MoveData>(clientId + 1, new Vector2Int(1, 1)).Object, playerId);
        
        messageSenderMock.Verify(sender => sender.Send(It.IsAny<Message>(), clientId), Times.Once);
    }

    [Theory]
    [InlineData(1, 3, 4)]
    [InlineData(2, 5, 6)]
    [InlineData(13, 213, 123)]
    public void SendDataToPlayer_ShouldSendMessage_WithAppropriateMoveFromServerDTO(int movePlayerId, int x, int y)
    {
        var messageSender = new TestMessageSenderById();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        var moveDataSender = new MoveDataSender(messageSender, clientIdProviderMock.Object);
        var moveData = new UpgradeMoveData(movePlayerId, new Vector2Int(x, y));
        var expectedDTO = new MoveFromServerDTO(moveData);
        
        moveDataSender.SendDataToPlayer(moveData, 1);
        var sentMessage = messageSender.SentMessage;
        sentMessage.GetByte();
        sentMessage.GetByte();
        var sentDTO = sentMessage.GetMoveFromServerDTO();
        
        Assert.Equal(expectedDTO, sentDTO);
    }

    [Fact]
    public void SendDataToPlayer_ShouldSendMessage_WithMakeMoveMessageId()
    {
        var messageSender = new TestMessageSenderById();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        var moveDataSender = new MoveDataSender(messageSender, clientIdProviderMock.Object);
        var moveData = new Mock<MoveData>(1, new Vector2Int(1, 1)).Object;
        
        moveDataSender.SendDataToPlayer(moveData, 1);
        var sentMessage = messageSender.SentMessage;
        var messageId = sentMessage.GetUShort();
        
        Assert.Equal((ushort)ServerToClientMessageType.MakeMove, messageId);
    }
}