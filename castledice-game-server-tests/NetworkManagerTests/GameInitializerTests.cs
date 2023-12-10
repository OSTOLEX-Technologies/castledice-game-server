using castledice_events_logic.ServerToClient;
using castledice_game_server.GameController.GameInitialization;
using castledice_game_server.NetworkManager;
using Moq;

namespace castledice_game_server_tests.NetworkManagerTests;

public class GameInitializerTests
{
    [Fact]
    public async void AcceptMatchFoundDTOAsync_ShouldPassPlayersIdsFromDTO_ToController()
    {
        var playersIdsToPass = new List<int>{ 3, 4 };
        var matchFoundDTO = new MatchFoundDTO(playersIdsToPass);
        var controllerMock = new Mock<IGameInitializationController>();
        controllerMock.Setup(controller => controller.InitializeGameAsync(playersIdsToPass))
            .Returns(Task.CompletedTask).Verifiable();
        var gameInitializer = new GameInitializer(controllerMock.Object);
        
        await gameInitializer.AcceptMatchFoundDTOAsync(matchFoundDTO);
        
        controllerMock.Verify(controller => controller.InitializeGameAsync(playersIdsToPass), Times.Once);
    }
}