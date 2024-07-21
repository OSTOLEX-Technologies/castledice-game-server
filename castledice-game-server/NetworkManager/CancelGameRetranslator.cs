using castledice_events_logic.ClientToServer;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class CancelGameRetranslator : ICancelGameDTOAccepter
{
    private readonly IMessageSender _messageSender;
    private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    public CancelGameRetranslator(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public void AcceptCancelGameDTO(CancelGameDTO dto, ushort clientId)
    {
        _logger.Debug($"Retranslating cancel game message for client with verification key: {dto.VerificationKey}");
        var message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerMessageType.CancelGame);
        message.AddCancelGameDTO(dto);
        _messageSender.Send(message);
    }
}