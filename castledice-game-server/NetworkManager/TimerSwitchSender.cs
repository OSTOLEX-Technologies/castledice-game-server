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
    private readonly IPlayerClientIdProvider _playerClientIdProvider;

    public TimerSwitchSender(IMessageSenderById messageSender, IPlayerClientIdProvider playerClientIdProvider)
    {
        _messageSender = messageSender;
        _playerClientIdProvider = playerClientIdProvider;
    }

    public void SendTimerSwitch(int playerToSwitchId, TimeSpan timeLeft, int accepterPlayerId, bool switchTo)
    {
        var clientId = _playerClientIdProvider.GetClientIdForPlayer(accepterPlayerId);
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.SwitchTimer);
        var DTO = new SwitchTimerDTO(timeLeft, playerToSwitchId, switchTo);
        message.AddSwitchTimerDTO(DTO);
        _messageSender.Send(message, clientId);
    }
}