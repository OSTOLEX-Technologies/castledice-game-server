using castledice_events_logic.ClientToServer;
using castledice_game_data_logic.Moves;
using castledice_game_logic.Math;
using castledice_game_server.GameController.Moves;
using castledice_game_server.NetworkManager;
using Moq;
using static castledice_game_server_tests.ObjectCreationUtility;

namespace castledice_game_server_tests.NetworkManagerTests;

public class MoveAccepterTests
{
    [Fact]
    public void AcceptMoveFromClientDTO_ShouldPassMoveDataFromDTO_ToGivenController()
    {
        var controllerMock = new Mock<IMovesController>();
        var expectedMoveData = new Mock<MoveData>(1, new Vector2Int(1, 1)).Object;
        var DTO = new MoveFromClientDTO(expectedMoveData, "sometoken");
        var moveAccepter = new MoveAccepter(controllerMock.Object);
        
        moveAccepter.AcceptMoveFromClientDTO(DTO);
        
        controllerMock.Verify(controller => controller.MakeMove(expectedMoveData));
    }
}