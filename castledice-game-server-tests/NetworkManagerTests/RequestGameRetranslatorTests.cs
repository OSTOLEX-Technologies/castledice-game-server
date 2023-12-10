using castledice_events_logic.ClientToServer;
using castledice_game_server_tests.TestImplementations;
using castledice_game_server.NetworkManager;
using castledice_riptide_dto_adapters.Extensions;

namespace castledice_game_server_tests.NetworkManagerTests;

public class RequestGameRetranslatorTests
{
    [Fact]
    public void AcceptRequestGameDTO_ShouldSendMessage_WithAcceptedDTO()
    {
        var DTO = new RequestGameDTO("somekey");
        var messageSender = new TestMessageSender();
        var retranslator = new RequestGameRetranslator(messageSender);
        
        retranslator.AcceptRequestGameDTO(DTO, 1);
        var sentMessage = messageSender.SentMessage;
        sentMessage.GetByte();
        sentMessage.GetByte();
        var sentDTO = sentMessage.GetRequestGameDTO();
        
        Assert.Equal(DTO, sentDTO);
    }
}