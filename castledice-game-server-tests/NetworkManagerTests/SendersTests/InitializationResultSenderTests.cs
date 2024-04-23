using castledice_events_logic.ServerToClient;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_game_server.NetworkManager.Senders;
using castledice_riptide_dto_adapters.Extensions;
using Moq;
using Riptide;

namespace castledice_game_server_tests.NetworkManagerTests.SendersTests;

public class InitializationResultSenderTests
{
    [Fact]
    public void SendInitializationResult_ShouldSendMessage_WithGivenDTO()
    {
        var expectedDto = new PlayerInitializationResultDTO(new Random().Next(0, 2) == 1);
        Message sentMessage = null;
        var messageSenderMock = new Mock<IMessageSenderById>();
        messageSenderMock.Setup(sender => sender.Send(It.IsAny<Message>(), It.IsAny<ushort>()))
            .Callback<Message, ushort>((message, _) => sentMessage = message);
        var initializationResultSender = new PlayerInitializationResultDTOSender(messageSenderMock.Object);
        
        initializationResultSender.SendInitializationResult(1, expectedDto);

        sentMessage.GetByte();
        var sentDto = sentMessage.GetPlayerInitializationResultDTO();
        
        Assert.Equal(expectedDto, sentDto);
    }

    [Fact]
    public void SendInitializationResult_ShouldSendMessage_WithInitializationResultMessageType()
    {
        Message sentMessage = null;
        var messageSenderMock = new Mock<IMessageSenderById>();
        messageSenderMock.Setup(sender => sender.Send(It.IsAny<Message>(), It.IsAny<ushort>()))
            .Callback<Message, ushort>((message, _) => sentMessage = message);
        var initializationResultSender = new PlayerInitializationResultDTOSender(messageSenderMock.Object);
        
        initializationResultSender.SendInitializationResult(1, new PlayerInitializationResultDTO(true));
        
        var sentMessageType = sentMessage.GetByte();
        Assert.Equal((ushort)ServerToClientMessageType.InitializationResult, sentMessageType);
    }

    [Fact]
    public void SendInitializationResult_ShouldSendMessage_WithReliableSendMode()
    {
        Message sentMessage = null;
        var messageSenderMock = new Mock<IMessageSenderById>();
        messageSenderMock.Setup(sender => sender.Send(It.IsAny<Message>(), It.IsAny<ushort>()))
            .Callback<Message, ushort>((message, _) => sentMessage = message);
        var initializationResultSender = new PlayerInitializationResultDTOSender(messageSenderMock.Object);
        
        initializationResultSender.SendInitializationResult(1, new PlayerInitializationResultDTO(true));
        
        var sentSendMode = sentMessage.SendMode;
        Assert.Equal(MessageSendMode.Reliable, sentSendMode);
    }
}