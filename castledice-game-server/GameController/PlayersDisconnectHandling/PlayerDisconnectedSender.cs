using castledice_events_logic.ServerToClient;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.GameController.PlayersDisconnectHandling;

public class PlayerDisconnectedSender : IPlayerDisconnectedSender
{
    private readonly IMessageSenderById _messageSender;
    private readonly IPlayerToClientIdsMap _idsMap;

    public PlayerDisconnectedSender(IMessageSenderById messageSender, IPlayerToClientIdsMap idsMap)
    {
        _messageSender = messageSender;
        _idsMap = idsMap;
    }

    public void SendPlayerDisconnectedMessage(int disconnectedPlayerId, int sendToPlayerId)
    {
        var dto = new PlayerDisconnectedDTO(disconnectedPlayerId);
        var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.PlayerDisconnected);
        message.AddPlayerDisconnectedDTO(dto);
        var clientId = _idsMap.GetClientByPlayer(sendToPlayerId);
        _messageSender.Send(message, clientId);
    }
}