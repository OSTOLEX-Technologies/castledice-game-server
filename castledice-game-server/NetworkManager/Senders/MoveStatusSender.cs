using castledice_events_logic.ServerToClient;
using castledice_game_server.GameController.Moves;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager.Senders;

public class MoveStatusSender : IMoveStatusSender
{
    private readonly IMessageSenderById _messageSender;
    private readonly IPlayerToClientIdsMap _idsMap;

    public MoveStatusSender(IMessageSenderById messageSender, IPlayerToClientIdsMap idsMap)
    {
        _messageSender = messageSender;
        _idsMap = idsMap;
    }

    public void SendMoveStatusToPlayer(bool isApproved, int playerId)
    {
        var clientId = _idsMap.GetClientByPlayer(playerId);
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.ApproveMove);
        var DTO = new ApproveMoveDTO(isApproved);
        message.AddApproveMoveDTO(DTO);
        _messageSender.Send(message, clientId);
    }
}