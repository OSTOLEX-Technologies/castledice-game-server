using castledice_events_logic.ServerToClient;
using castledice_game_data_logic.Moves;
using castledice_game_server.GameController.Moves;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class MoveDataSender : IMoveDataSender
{
    private readonly IMessageSenderById _messageSender;
    private readonly IPlayerClientIdProvider _playerClientIdProvider;

    public MoveDataSender(IMessageSenderById messageSender, IPlayerClientIdProvider playerClientIdProvider)
    {
        _messageSender = messageSender;
        _playerClientIdProvider = playerClientIdProvider;
    }

    public void SendDataToPlayer(MoveData moveData, int playerId)
    {
        var clientId = _playerClientIdProvider.GetClientIdForPlayer(playerId);
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.MakeMove);
        var DTO = new MoveFromServerDTO(moveData);
        message.AddMoveFromServerDTO(DTO);
        _messageSender.Send(message, clientId);
    }
}