using castledice_events_logic.ClientToServer;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class RequestGameRetranslator : IRequestGameDTOAccepter
{
    private readonly IMessageSender _messageSender;

    public RequestGameRetranslator(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public void AcceptRequestGameDTO(RequestGameDTO requestGameDTO, ushort clientId)
    {
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerMessageType.RequestGame);
        message.AddRequestGameDTO(requestGameDTO);
        _messageSender.Send(message);
    }
}