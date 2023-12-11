using castledice_events_logic.ClientToServer;
using castledice_game_server.GameController.PlayerInitialization;
using castledice_game_server.NetworkManager;
using Moq;

namespace castledice_game_server_tests.NetworkManagerTests;

public class PlayerInitializerTests
{
    [Theory]
    [InlineData("playerToken", 1)]
    [InlineData("anotherPlayerToken", 2)]
    [InlineData("yetAnotherPlayerToken", 3)]
    [InlineData("andAnotherPlayerToken", 4)]
    public async void AcceptInitializePlayerDTO_ShouldPassPlayerTokenFromDTOAndClientId_ToGivenController(string playerToken, ushort clientId)
    {
        var controllerMock = new Mock<IPlayerInitializationController>();
        controllerMock.Setup(controller => controller.InitializePlayerAsync(playerToken, clientId))
            .Returns(Task.CompletedTask).Verifiable();
        var initializer = new PlayerInitializer(controllerMock.Object);
        var dto = new InitializePlayerDTO(playerToken);
        
        await initializer.AcceptInitializePlayerDTOAsync(dto, clientId);
        
        controllerMock.Verify(controller => controller.InitializePlayerAsync(playerToken, clientId), Times.Once);
    }
}