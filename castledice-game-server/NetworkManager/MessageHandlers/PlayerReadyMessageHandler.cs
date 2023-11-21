using casltedice_events_logic.ClientToServer;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager.MessageHandlers;

public static class PlayerReadyMessageHandler
{
    private static IPlayerReadyDTOAccepter _dtoAccepter;
    
    public static void SetAccepter(IPlayerReadyDTOAccepter dtoAccepter)
    {
        _dtoAccepter = dtoAccepter;
    }

    [MessageHandler((ushort)ClientToServerMessageType.PlayerReady)]
    private static void HandlePlayerReadyMessage(ushort fromClientId, Message message)
    {
        var dto = message.GetPlayerReadyDTO();
        Task.Run(() => _dtoAccepter.AcceptPlayerReadyDTOAsync(dto));
    }
}