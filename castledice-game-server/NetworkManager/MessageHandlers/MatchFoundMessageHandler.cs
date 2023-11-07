﻿using casltedice_events_logic.ServerToClient;
using castledice_game_server.NetworkManager.DTOAccepters;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager.MessageHandlers;

public static class MatchFoundMessageHandler
{
    private static IMatchFoundDTOAccepter _dtoAccepter;
    
    public static void SetDTOAccepter(IMatchFoundDTOAccepter dtoAccepter)
    {
        _dtoAccepter = dtoAccepter;
    }

    [MessageHandler((ushort)ServerToClientMessageType.MatchFound)]
    private static void HandleMatchFoundMessage(Message message)
    {
        var dto = message.GetMatchFoundDTO();
        Task.Run(() => _dtoAccepter.AcceptMatchFoundDTOAsync(dto));
    }
}