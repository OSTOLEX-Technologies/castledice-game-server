using castledice_events_logic.ServerToClient;
using castledice_game_server.GameController.Moves;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class MoveStatusSender : IMoveStatusSender
{
    private readonly IMessageSenderById _messageSender;
    private readonly IPlayerClientIdProvider _playerClientIdProvider;

    public MoveStatusSender(IMessageSenderById messageSender, IPlayerClientIdProvider playerClientIdProvider)
    {
        _messageSender = messageSender;
        _playerClientIdProvider = playerClientIdProvider;
    }

    public void SendMoveStatusToPlayer(bool isApproved, int playerId)
    {
        var clientId = _playerClientIdProvider.GetClientIdForPlayer(playerId);
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.ApproveMove);
        var DTO = new ApproveMoveDTO(isApproved);
        message.AddApproveMoveDTO(DTO);
        _messageSender.Send(message, clientId);
    }
}