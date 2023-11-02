using casltedice_events_logic.ClientToServer;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager.MessageHandlers;

public static class CancelGameMessageHandler
{
    private static ICancelGameDTOAccepter _dtoAccepter;
    
    public static void SetAccepter(ICancelGameDTOAccepter dtoAccepter)
    {
        _dtoAccepter = dtoAccepter;
    }
    
    [MessageHandler((ushort)ClientToServerMessageType.CancelGame)]
    private static void HandleCancelGameMessage(ushort fromClientId, Message message)
    {
        _dtoAccepter.AcceptCancelGameDTO(message.GetCancelGameDTO(), fromClientId);
    }
}