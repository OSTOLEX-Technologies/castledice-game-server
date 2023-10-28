using casltedice_events_logic.ServerToClient;
using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_riptide_dto_adapters.Extensions;
using Moq;
using Riptide;

namespace castledice_game_server_tests;

public class CancelGameResultRetranslatorTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    //AcceptCancelGameResultDTO should find client id by using given IPlayerClientIdProvider
    public void AcceptCancelGameResultDTO_ShouldSendMessageWithAcceptedDTO_ToClientWithAppropriateId(int playerId, ushort clientId)
    {
        var DTO = new CancelGameResultDTO(false, playerId);
        var messageSenderMock = new Mock<IMessageSenderById>();
        var clientIdProviderMock = new Mock<IPlayerClientIdProvider>();
        clientIdProviderMock.Setup(provider => provider.GetClientIdForPlayer(playerId)).Returns(clientId);
        var retranslator = new CancelGameResultRetranslator(messageSenderMock.Object, clientIdProviderMock.Object);
        
        retranslator.AcceptCancelGameResultDTO(DTO);
        
        //Next line verifies two things. First, that Send methods was called with appropriate id. Second, that message contains appropriate dto.
        messageSenderMock.Verify(x => x.Send(It.Is<Message>(m => MessageContainsCancelGameResultDTO(m, DTO)), clientId), Times.Once);
    }
    
    private static bool MessageContainsCancelGameResultDTO(Message message, CancelGameResultDTO dto)
    {
        message.GetByte();
        message.GetByte();
        var messageDTO = message.GetCancelGameResultDTO();
        return messageDTO.Equals(dto);
    }
}