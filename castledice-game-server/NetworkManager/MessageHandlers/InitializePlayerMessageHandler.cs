using casltedice_events_logic.ClientToServer;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager.MessageHandlers;

public static class InitializePlayerMessageHandler
{
    private static PlayerInitializer _playerInitializer;

    public static void SetPlayerInitializer(PlayerInitializer initializer)
    {
        _playerInitializer = initializer;
    }
    
    [MessageHandler((ushort)ClientToServerMessageType.InitializePlayer)]
    public static void HandleInitializePlayerMessage(Message message)
    {
        
    }
}