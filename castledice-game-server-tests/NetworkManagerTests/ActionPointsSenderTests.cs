using castledice_events_logic.ServerToClient;
using castledice_game_server_tests.TestImplementations;
using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_riptide_dto_adapters.Extensions;
using Moq;
using Riptide;

namespace castledice_game_server_tests.NetworkManagerTests;

public class ActionPointsSenderTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 13)]
    [InlineData(3, 123)]
    public void SendActionPoints_ShouldSendMessage_ToAppropriateClient(int messageAccepterId, ushort clientId)
    {
        var clientProviderMock = new Mock<IPlayerClientIdProvider>();
        clientProviderMock.Setup(provider => provider.GetClientIdForPlayer(messageAccepterId)).Returns(clientId);
        var messageSenderMock = new Mock<IMessageSenderById>();
        var actionPointsSender = new ActionPointsSender(messageSenderMock.Object, clientProviderMock.Object);
        
        actionPointsSender.SendActionPoints(1, 1, messageAccepterId);
        
        messageSenderMock.Verify(sender => sender.Send(It.IsAny<Message>(), clientId), Times.Once);
    }

    [Theory]
    [InlineData(1, 1, 3)]
    [InlineData(2, 13, 2)]
    [InlineData(3, 123, 3154)]
    public void SendActionPoints_ShouldSendMessage_WithAppropriateGiveActionPointsDTO(int amount,
        int actionPointsAccepterId, int messageAccepterId)
    {
        var clientProviderMock = new Mock<IPlayerClientIdProvider>();
        var messageSender = new TestMessageSenderById();
        var actionPointsSender = new ActionPointsSender(messageSender, clientProviderMock.Object);
        var expectedDTO = new GiveActionPointsDTO(actionPointsAccepterId, amount);
        
        actionPointsSender.SendActionPoints(amount, actionPointsAccepterId, messageAccepterId);
        var sentMessage = messageSender.SentMessage;
        sentMessage.GetByte();
        sentMessage.GetByte();
        var sentDTO = sentMessage.GetGiveActionPointsDTO();
        
        Assert.Equal(expectedDTO, sentDTO);
    }
    
    [Fact]
    public void SendActionPoints_ShouldSendMessage_WithGiveActionPointsMessageId()
    {
        var clientProviderMock = new Mock<IPlayerClientIdProvider>();
        var messageSender = new TestMessageSenderById();
        var actionPointsSender = new ActionPointsSender(messageSender, clientProviderMock.Object);
        
        actionPointsSender.SendActionPoints(1, 1, 1);
        var sentMessage = messageSender.SentMessage;
        var sentMessageId = sentMessage.GetUShort();
        
        Assert.Equal((ushort)ServerToClientMessageType.GiveActionPoints, sentMessageId);
    }
}