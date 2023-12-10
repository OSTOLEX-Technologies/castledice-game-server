using castledice_events_logic.ClientToServer;
using castledice_game_server_tests.TestImplementations;
using castledice_game_server.NetworkManager;
using castledice_riptide_dto_adapters.Extensions;

namespace castledice_game_server_tests.NetworkManagerTests;

public class CancelGameRetranslatorTests
{
    [Fact]
    public void AcceptCancelGameDTO_ShouldSendMessage_WithAcceptedDTO()
    {
        var DTO = new CancelGameDTO("somekey");
        var messageSender = new TestMessageSender();
        var retranslator = new CancelGameRetranslator(messageSender);
        
        retranslator.AcceptCancelGameDTO(DTO, 1);
        var sentMessage = messageSender.SentMessage;
        sentMessage.GetByte();
        sentMessage.GetByte();
        var sentDTO = sentMessage.GetCancelGameDTO();
        
        Assert.Equal(DTO, sentDTO);
    }
}