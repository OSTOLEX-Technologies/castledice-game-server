using castledice_events_logic.ServerToClient;
using castledice_events_logic.ServerToClient;
using castledice_game_data_logic.Errors;
using castledice_game_server.GameController;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class ErrorSender : IErrorSender
{
    private readonly IMessageSenderById _messageSenderById;
    private readonly IPlayerToClientIdsMap _idsMap;

    public ErrorSender(IMessageSenderById messageSenderById, IPlayerToClientIdsMap idsMap)
    {
        _messageSenderById = messageSenderById;
        _idsMap = idsMap;
    }

    public void SendErrorToPlayer(ErrorData errorData, int playerId)
    {
        var clientId = _idsMap.GetClientByPlayer(playerId);
        var serverErrorDTO = new ServerErrorDTO(errorData);
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.Error);
        message.AddServerErrorDTO(serverErrorDTO);
        _messageSenderById.Send(message, clientId);
    }
}