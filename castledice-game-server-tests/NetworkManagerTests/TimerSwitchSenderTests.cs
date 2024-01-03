using castledice_events_logic.ServerToClient;
using castledice_game_server_tests.TestImplementations;
using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Moq;
using Riptide;

namespace castledice_game_server_tests.NetworkManagerTests;

public class TimerSwitchSenderTests
{
    [Fact]
    public void SendTimerSwitch_ShouldSendMessage_ToAccepterClient()
    {
        var random = new Random();
        var accepterId = random.Next();
        var expectedClientId = (ushort)random.Next(ushort.MaxValue);
        var messageSenderMock = new Mock<IMessageSenderById>();
        var playerClientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        playerClientIdProviderMock.Setup(x => x.GetClientIdForPlayer(accepterId)).Returns(expectedClientId);
        var sender = new TimerSwitchSenderBuilder
        {
            MessageSender = messageSenderMock.Object,
            PlayerClientIdProvider = playerClientIdProviderMock.Object
        }.Build();
        
        sender.SendTimerSwitch(0, TimeSpan.Zero, accepterId, true);
        
        messageSenderMock.Verify(x => x.Send(It.IsAny<Message>(), expectedClientId), Times.Once);
    }

    [Fact]
    public void SendTimerSwitch_ShouldSendMessage_WithSwitchTimerId()
    {
        var messageSender = new TestMessageSenderById();
        var sender = new TimerSwitchSenderBuilder {MessageSender = messageSender}.Build();
        
        sender.SendTimerSwitch(0, TimeSpan.Zero, 1, true);
        var sentMessage = messageSender.SentMessage;
        
        Assert.Equal((ushort)ServerToClientMessageType.SwitchTimer, sentMessage.GetUShort());
    }
    
    [Fact]
    public void SendTimerSwitch_ShouldSendSwitchTimerDTO_WithAppropriateData()
    {
        var random = new Random();
        var timeLeft = TimeSpan.FromSeconds(random.Next());
        var playerToSwitchId = random.Next();
        var switchTo = random.Next() % 2 == 0;
        var messageSender = new TestMessageSenderById();
        var sender = new TimerSwitchSenderBuilder{MessageSender = messageSender}.Build();
        var expectedDTO = new SwitchTimerDTO(timeLeft, playerToSwitchId, switchTo);
        
        sender.SendTimerSwitch(playerToSwitchId, timeLeft, 0, switchTo);
        var sentMessage = messageSender.SentMessage;
        sentMessage.GetByte();
        sentMessage.GetByte();
        var actualDTO = sentMessage.GetSwitchTimerDTO();
        
        Assert.Equal(expectedDTO, actualDTO);
    }

    private class TimerSwitchSenderBuilder
    {
        public IMessageSenderById MessageSender { get; set; } = new Mock<IMessageSenderById>().Object;
        public IPlayerClientIdProvider PlayerClientIdProvider { get; set; } = new Mock<IPlayerClientIdProvider>().Object;
        
        public TimerSwitchSender Build()
        {
            return new TimerSwitchSender(MessageSender, PlayerClientIdProvider);
        }
    }
}