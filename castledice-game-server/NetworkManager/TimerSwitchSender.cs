using castledice_events_logic.ServerToClient;
using castledice_game_server.GameController.Timers;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class TimerSwitchSender : ITimerSwitchSender
{
    private readonly IMessageSenderById _messageSender;
    private readonly IPlayerToClientIdsMap _idsMap;

    public TimerSwitchSender(IMessageSenderById messageSender, IPlayerToClientIdsMap idsMap)
    {
        _messageSender = messageSender;
        _idsMap = idsMap;
    }

    public void SendTimerSwitch(int playerToSwitchId, TimeSpan timeLeft, int accepterPlayerId, bool switchTo)
    {
        var clientId = _idsMap.GetClientByPlayer(accepterPlayerId);
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.SwitchTimer);
        var DTO = new SwitchTimerDTO(timeLeft, playerToSwitchId, switchTo);
        message.AddSwitchTimerDTO(DTO);
        _messageSender.Send(message, clientId);
    }
}