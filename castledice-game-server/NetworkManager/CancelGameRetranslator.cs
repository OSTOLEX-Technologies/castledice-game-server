using castledice_events_logic.ClientToServer;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class CancelGameRetranslator : ICancelGameDTOAccepter
{
    private readonly IMessageSender _messageSender;

    public CancelGameRetranslator(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public void AcceptCancelGameDTO(CancelGameDTO dto, ushort clientId)
    {
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerMessageType.CancelGame);
        message.AddCancelGameDTO(dto);
        _messageSender.Send(message);
    }
}