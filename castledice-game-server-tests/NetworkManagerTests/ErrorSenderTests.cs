using castledice_events_logic.ServerToClient;
using castledice_game_data_logic.Errors;
using castledice_game_server_tests.TestImplementations;
using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Moq;
using Riptide;

namespace castledice_game_server_tests.NetworkManagerTests;

public class ErrorSenderTests
{
    [Fact]
    public void SendErrorToPlayer_ShouldSendMessageOnce()
    {
        var messageSenderMock = new Mock<IMessageSenderById>();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        var errorSender = new ErrorSender(messageSenderMock.Object, clientIdProviderMock.Object);
        
        errorSender.SendErrorToPlayer(new ErrorData(ErrorType.GameNotSaved, "Test error message"), 1);
        
        messageSenderMock.Verify(ms => ms.Send(It.IsAny<Message>(), It.IsAny<ushort>()), Times.Once);
    }
    
    [Theory]
    [InlineData("Test error message", ErrorType.GameNotSaved)]
    [InlineData("Some test error message", ErrorType.GameNotSaved)]
    [InlineData("Test error message 1234", ErrorType.GameNotSaved)]
    public void SendErrorToPlayer_ShouldSendProperServerErrorDTO(string errorMessage, ErrorType errorType)
    {
        var messageSender = new TestMessageSenderById();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        var expectedDTO = new ServerErrorDTO(new ErrorData(errorType, errorMessage));
        var errorSender = new ErrorSender(messageSender, clientIdProviderMock.Object);
        
        errorSender.SendErrorToPlayer(new ErrorData(errorType, errorMessage), 1);
        var sentMessage = messageSender.SentMessage;
        sentMessage.GetByte();
        sentMessage.GetByte();
        var sentDTO = sentMessage.GetServerErrorDTO();
        
        Assert.Equal(expectedDTO.ErrorData, sentDTO.ErrorData);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3134, 5315)]
    public void SendErrorToPlayer_ShouldSendMessageToClientId_FromGivenProvider(int playerId, ushort clientId)
    {
        var messageSenderMock = new Mock<IMessageSenderById>();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        clientIdProviderMock.Setup(provider => provider.GetClientIdForPlayer(playerId)).Returns(clientId);
        var errorSender = new ErrorSender(messageSenderMock.Object, clientIdProviderMock.Object);
        
        errorSender.SendErrorToPlayer(new ErrorData(ErrorType.GameNotSaved, "Test error message"), playerId);
        
        messageSenderMock.Verify(ms => ms.Send(It.IsAny<Message>(), clientId), Times.Once);
    }

    [Fact]
    public void SendErrorToPlayer_ShouldSendMessage_WithErrorMessageId()
    {
        var messageSender = new TestMessageSenderById();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        var errorSender = new ErrorSender(messageSender, clientIdProviderMock.Object);
        
        errorSender.SendErrorToPlayer(new ErrorData(ErrorType.GameNotSaved, "Test error message"), 1);
        var sentMessage = messageSender.SentMessage;
        var sentMessageId = sentMessage.GetByte();
        
        Assert.Equal((byte)ServerToClientMessageType.Error, sentMessageId);
    }
}