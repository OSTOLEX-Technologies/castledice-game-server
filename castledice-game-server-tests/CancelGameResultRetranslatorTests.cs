using casltedice_events_logic.ServerToClient;
using castledice_game_server_tests.TestImplementations;
using castledice_game_server.NetworkManager;
using castledice_riptide_dto_adapters.Extensions;
using Moq;
using Riptide;

namespace castledice_game_server_tests;

public class CancelGameResultRetranslatorTests
{
    [Fact]
    public void AcceptCancelGameResultDTO_ShouldThrowInvalidOperationException_IfPlayerIdIsAbsentInDictionary()
    {
        var DTO = new CancelGameResultDTO(false, 3);
        var messageSender = new TestMessageSenderById();
        var retranslator = new CancelGameResultRetranslator(messageSender);
        
        Assert.Throws<InvalidOperationException>(() => retranslator.AcceptCancelGameResultDTO(DTO));
    }

    [Fact]
    //AcceptCancelGameResultDTO should find client id in PlayersDictionary by using id from dto and then send message to client with this id.
    public void AcceptCancelGameResultDTO_ShouldSendMessageWithAcceptedDTO_ToClientWithAppropriateId()
    {
        var playerId = 3;
        var playerClientId = (ushort)4;
        PlayersDictionary.Dictionary.Add(playerClientId, playerClientId);
        var DTO = new CancelGameResultDTO(false, playerClientId);
        var messageSenderMock = new Mock<IMessageSenderById>();
        var retranslator = new CancelGameResultRetranslator(messageSenderMock.Object);
        
        retranslator.AcceptCancelGameResultDTO(DTO);
        
        //Next line verifies two things. First, that Send methods was called with appropriate id. Second, that message contains appropriate dto.
        messageSenderMock.Verify(x => x.Send(It.Is<Message>(m => MessageContainsCancelGameResultDTO(m, DTO)), playerClientId), Times.Once);
        PlayersDictionary.Dictionary.Clear();
    }
    
    private static bool MessageContainsCancelGameResultDTO(Message message, CancelGameResultDTO dto)
    {
        message.GetByte();
        message.GetByte();
        var messageDTO = message.GetCancelGameResultDTO();
        return messageDTO.Equals(dto);
    }
}