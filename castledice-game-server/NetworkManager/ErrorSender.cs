using casltedice_events_logic.ServerToClient;
using castledice_game_data_logic.Errors;
using castledice_game_server.GameController;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class ErrorSender : IErrorSender
{
    private readonly IMessageSenderById _messageSenderById;
    private readonly IPlayerClientIdProvider _playerClientIdProvider;

    public ErrorSender(IMessageSenderById messageSenderById, IPlayerClientIdProvider playerClientIdProvider)
    {
        _messageSenderById = messageSenderById;
        _playerClientIdProvider = playerClientIdProvider;
    }

    public void SendErrorToPlayer(ErrorData errorData, int playerId)
    {
        var clientId = _playerClientIdProvider.GetClientIdForPlayer(playerId);
        var serverErrorDTO = new ServerErrorDTO(errorData);
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.Error);
        message.AddServerErrorDTO(serverErrorDTO);
        _messageSenderById.Send(message, clientId);
    }
}