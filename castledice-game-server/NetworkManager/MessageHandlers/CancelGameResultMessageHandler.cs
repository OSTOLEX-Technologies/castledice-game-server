using casltedice_events_logic.ServerToClient;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager.MessageHandlers;

public class CancelGameResultMessageHandler
{
    private static ICancelGameResultDTOAccepter _dtoAccepter;
    
    public static void SetAccepter(ICancelGameResultDTOAccepter dtoAccepter)
    {
        _dtoAccepter = dtoAccepter;
    }
    
    [MessageHandler((ushort)ServerToClientMessageType.CancelGame)]
    public static void HandleCancelGameResultMessage(Message message)
    {
        _dtoAccepter.AcceptCancelGameResultDTO(message.GetCancelGameResultDTO());
    }
}