using castledice_events_logic.ServerToClient;
using castledice_game_server.GameController.ActionPoints;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager.Senders;

public class ActionPointsSender : IActionPointsSender
{
    private readonly IMessageSenderById _messageSender;
    private readonly IPlayerToClientIdsMap _idsMap;

    public ActionPointsSender(IMessageSenderById messageSender, IPlayerToClientIdsMap idsMap)
    {
        _messageSender = messageSender;
        _idsMap = idsMap;
    }

    public void SendActionPoints(int amount, int actionPointsAccepterId, int messageAccepterId)
    {
        var clientId = _idsMap.GetClientByPlayer(messageAccepterId);
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.GiveActionPoints);
        var DTO = new GiveActionPointsDTO(actionPointsAccepterId, amount);
        message.AddGiveActionPointsDTO(DTO);
        _messageSender.Send(message, clientId);
    }
}