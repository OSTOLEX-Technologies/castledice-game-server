﻿using casltedice_events_logic.ServerToClient;
using castledice_game_server.GameController;
using castledice_game_server.NetworkManager;
using Moq;

namespace castledice_game_server_tests;

public class GameInitializerTests
{
    [Fact]
    public void AcceptMatchFoundDTO_ShouldPassPlayersIdsFromDTO_ToController()
    {
        var playersIdsToPass = new List<int>{ 3, 4 };
        var matchFoundDTO = new MatchFoundDTO(playersIdsToPass);
        var controllerMock = new Mock<IGameInitializationController>();
        var gameInitializer = new GameInitializer(controllerMock.Object);
        
        gameInitializer.AcceptMatchFoundDTO(matchFoundDTO);
        
        controllerMock.Verify(controller => controller.InitializeGame(playersIdsToPass), Times.Once);
    }
}