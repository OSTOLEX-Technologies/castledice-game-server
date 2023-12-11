using castledice_events_logic.ClientToServer;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager.MessageHandlers;

public static class InitializePlayerMessageHandler
{
    private static IInitializePlayerDTOAccepter _dtoAccepter;

    public static void SetDTOAccepter(IInitializePlayerDTOAccepter accepter)
    {
        _dtoAccepter = accepter;
    }
    
    [MessageHandler((ushort)ClientToServerMessageType.InitializePlayer)]
    private static void HandleInitializePlayerMessage(ushort fromClientId, Message message)
    {
        Task.Run(() => _dtoAccepter.AcceptInitializePlayerDTOAsync(message.GetInitializePlayerDTO(), fromClientId));
    }
}