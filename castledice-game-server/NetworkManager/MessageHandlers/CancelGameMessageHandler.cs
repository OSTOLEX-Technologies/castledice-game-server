using castledice_events_logic.ClientToServer;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_riptide_dto_adapters.Extensions;
using NLog;
using Riptide;

namespace castledice_game_server.NetworkManager.MessageHandlers;

public static class CancelGameMessageHandler
{
    private static ICancelGameDTOAccepter _dtoAccepter;
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    
    public static void SetAccepter(ICancelGameDTOAccepter dtoAccepter)
    {
        _dtoAccepter = dtoAccepter;
    }
    
    [MessageHandler((ushort)ClientToServerMessageType.CancelGame)]
    private static void HandleCancelGameMessage(ushort fromClientId, Message message)
    {
        _logger.Info($"Handling cancel game message for client id {fromClientId}");
        _dtoAccepter.AcceptCancelGameDTO(message.GetCancelGameDTO(), fromClientId);
    }
}