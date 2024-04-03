using castledice_events_logic.ServerToClient;
using castledice_game_server.GameController.PlayerInitialization;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager.Senders;

public class PlayerInitializationResultDTOSender : IPlayerInitializationResultDTOSender
{
    private readonly IMessageSenderById _messageSender;

    public PlayerInitializationResultDTOSender(IMessageSenderById messageSender)
    {
        _messageSender = messageSender;
    }

    public void SendInitializationResult(ushort clientId, PlayerInitializationResultDTO dto)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.InitializationResult);
        message.AddPlayerInitializationResultDTO(dto);
        _messageSender.Send(message, clientId);
    }
}