using castledice_events_logic.ServerToClient;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_riptide_dto_adapters.Extensions;
using NLog;
using Riptide;

namespace castledice_game_server.NetworkManager.MessageHandlers;

public static class CancelGameResultMessageHandler
{
    private static ICancelGameResultDTOAccepter _dtoAccepter;
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    
    public static void SetAccepter(ICancelGameResultDTOAccepter dtoAccepter)
    {
        _dtoAccepter = dtoAccepter;
    }
    
    [MessageHandler((ushort)ServerToClientMessageType.CancelGame)]
    private static void HandleCancelGameResultMessage(Message message)
    {
        _logger.Debug("Handling cancel game result message");
        _dtoAccepter.AcceptCancelGameResultDTO(message.GetCancelGameResultDTO());
    }
}