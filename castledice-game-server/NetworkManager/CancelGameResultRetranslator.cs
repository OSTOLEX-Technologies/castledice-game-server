using casltedice_events_logic.ServerToClient;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class CancelGameResultRetranslator : ICancelGameResultDTOAccepter
{
    private readonly IMessageSenderById _messageSender;

    public CancelGameResultRetranslator(IMessageSenderById messageSender)
    {
        _messageSender = messageSender;
    }

    public void AcceptCancelGameResultDTO(CancelGameResultDTO dto)
    {
        var playerId = dto.PlayerId;
        if (!PlayersDictionary.Dictionary.ContainsKey(playerId))
        {
            throw new InvalidOperationException("No client id found for player with id: " + playerId);
        }

        var clientId = PlayersDictionary.Dictionary[playerId];
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.CancelGame);
        message.AddCancelGameResultDTO(dto);
        _messageSender.Send(message, clientId);
    }
}