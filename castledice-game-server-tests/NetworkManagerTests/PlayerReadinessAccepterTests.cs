using castledice_events_logic.ClientToServer;
using castledice_game_server.GameController.PlayersReadiness;
using castledice_game_server.NetworkManager;
using Moq;

namespace castledice_game_server_tests.NetworkManagerTests;

public class PlayerReadinessAccepterTests
{
    [Theory]
    [InlineData("token")]
    [InlineData("anotherToken")]
    [InlineData("yetAnotherToken")]
    public async void AcceptPlayerReadyDTO_ShouldPassTokenFromDTO_ToGivenController(string token)
    {
        var controllerMock = new Mock<IPlayersReadinessController>();
        controllerMock.Setup(controller => controller.SetPlayerReadyAsync(token))
            .Returns(Task.CompletedTask).Verifiable();
        var accepter = new PlayerReadinessAccepter(controllerMock.Object);
        var dto = new PlayerReadyDTO(token);
        
        await accepter.AcceptPlayerReadyDTOAsync(dto);
        
        controllerMock.Verify(controller => controller.SetPlayerReadyAsync(token), Times.Once);
    }
}