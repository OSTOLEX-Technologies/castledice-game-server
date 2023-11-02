using casltedice_events_logic.ClientToServer;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager.MessageHandlers;

public static class MoveFromClientMessageHandler
{
    private static IMoveFromClientDTOAccepter _dtoAccepter;

    public static void SetAccepter(IMoveFromClientDTOAccepter accepter)
    {
        _dtoAccepter = accepter;
    }

    [MessageHandler((ushort)ClientToServerMessageType.MakeMove)]
    private static void HandleMoveFromClientMessage(Message message)
    {
        _dtoAccepter.AcceptMoveFromClientDTO(message.GetMoveFromClientDTO());
    }
}