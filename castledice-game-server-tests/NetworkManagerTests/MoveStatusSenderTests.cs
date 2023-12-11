using castledice_events_logic.ServerToClient;
using castledice_game_server_tests.TestImplementations;
using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_riptide_dto_adapters.Extensions;
using Moq;
using Riptide;

namespace castledice_game_server_tests.NetworkManagerTests;

public class MoveStatusSenderTests
{
    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 3)]
    [InlineData(13, 15)]
    public void SendMoveStatusToPlayer_ShouldSendMessage_ToAppropriateClientId(int playerId, ushort clientId)
    {
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        clientIdProviderMock.Setup(provider => provider.GetClientIdForPlayer(playerId)).Returns(clientId);
        var messageSenderMock = new Mock<IMessageSenderById>();
        var moveStatusSender = new MoveStatusSender(messageSenderMock.Object, clientIdProviderMock.Object);
        
        moveStatusSender.SendMoveStatusToPlayer(true, playerId);
        
        messageSenderMock.Verify(sender => sender.Send(It.IsAny<Message>(), clientId), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SendMoveStatusToPlayer_ShouldSendMessage_WithAppropriateApproveMoveDTO(bool isMoveValid)
    {
        var messageSender = new TestMessageSenderById();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        var moveStatusSender = new MoveStatusSender(messageSender, clientIdProviderMock.Object);
        var expectedDTO = new ApproveMoveDTO(isMoveValid);
        
        moveStatusSender.SendMoveStatusToPlayer(isMoveValid, 1);
        var sentMessage = messageSender.SentMessage;
        sentMessage.GetByte();
        sentMessage.GetByte();
        var sentDTO = sentMessage.GetApproveMoveDTO();
        
        Assert.Equal(expectedDTO, sentDTO);
    }
    
    [Fact]
    public void SendMoveStatusToPlayer_ShouldSendMessage_WithApproveMoveMessageId()
    {
        var messageSender = new TestMessageSenderById();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        var moveStatusSender = new MoveStatusSender(messageSender, clientIdProviderMock.Object);
        
        moveStatusSender.SendMoveStatusToPlayer(true, 1);
        var sentMessage = messageSender.SentMessage;
        var sentMessageId = sentMessage.GetUShort();
        
        Assert.Equal((ushort)ServerToClientMessageType.ApproveMove, sentMessageId);
    }
}