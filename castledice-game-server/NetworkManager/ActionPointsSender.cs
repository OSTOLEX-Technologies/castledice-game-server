using castledice_events_logic.ServerToClient;
using castledice_game_server.GameController.ActionPoints;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class ActionPointsSender : IActionPointsSender
{
    private readonly IMessageSenderById _messageSender;
    private readonly IPlayerClientIdProvider _playerClientIdProvider;

    public ActionPointsSender(IMessageSenderById messageSender, IPlayerClientIdProvider playerClientIdProvider)
    {
        _messageSender = messageSender;
        _playerClientIdProvider = playerClientIdProvider;
    }

    public void SendActionPoints(int amount, int actionPointsAccepterId, int messageAccepterId)
    {
        var clientId = _playerClientIdProvider.GetClientIdForPlayer(messageAccepterId);
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.GiveActionPoints);
        var DTO = new GiveActionPointsDTO(actionPointsAccepterId, amount);
        message.AddGiveActionPointsDTO(DTO);
        _messageSender.Send(message, clientId);
    }
}