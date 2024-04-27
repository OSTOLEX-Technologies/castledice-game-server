using castledice_events_logic.ServerToClient;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class CancelGameResultRetranslator : ICancelGameResultDTOAccepter
{
    private readonly IMessageSenderById _messageSender;
    private readonly IPlayerToClientIdsMap _idsMap;

    public CancelGameResultRetranslator(IMessageSenderById messageSender, IPlayerToClientIdsMap idsMap)
    {
        _messageSender = messageSender;
        _idsMap = idsMap;
    }

    public void AcceptCancelGameResultDTO(CancelGameResultDTO dto)
    {
        var playerId = dto.PlayerId;
        var clientId = _idsMap.GetClientByPlayer(playerId);
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.CancelGame);
        message.AddCancelGameResultDTO(dto);
        _messageSender.Send(message, clientId);
    }
}