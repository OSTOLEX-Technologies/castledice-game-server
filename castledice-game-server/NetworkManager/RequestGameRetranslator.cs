using castledice_events_logic.ClientToServer;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class RequestGameRetranslator : IRequestGameDTOAccepter
{
    private readonly IMessageSender _messageSender;
    private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    public RequestGameRetranslator(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public void AcceptRequestGameDTO(RequestGameDTO requestGameDTO, ushort clientId)
    {
        _logger.Debug($"Retranslating request game message for client with verification key: {requestGameDTO.VerificationKey}");
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerMessageType.RequestGame);
        message.AddRequestGameDTO(requestGameDTO);
        _messageSender.Send(message);
    }
}