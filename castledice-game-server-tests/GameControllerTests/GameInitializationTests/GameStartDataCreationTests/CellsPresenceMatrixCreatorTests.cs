using castledice_game_logic;
using castledice_game_logic.Math;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests;

public class CellsPresenceMatrixCreatorTests
{
    [Theory]
    [MemberData(nameof(GetCellsPresenceMatrixTestCases))]
    public void GetCellsPresenceMatrix_ShouldReturnAppropriateCellsPresenceMatrix(Board board, bool[,] expectedMatrix)
    {
        var creator = new CellsPresenceMatrixCreator();
        
        var actualMatrix = creator.GetCellsPresenceMatrix(board);

        for (int i = 0; i < expectedMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < expectedMatrix.GetLength(1); j++)
            {
                Assert.Equal(expectedMatrix[i, j], actualMatrix[i, j]);
            }
        }
    }

    public static IEnumerable<object[]> GetCellsPresenceMatrixTestCases()
    {
        yield return GetFullBoardCase(10, 10);
        yield return GetFullBoardCase(5, 5);
        yield return GetFullBoardCase(1, 1);
        yield return GetArbitraryBoardCase(new Vector2Int(1, 1), new Vector2Int(2, 2), new Vector2Int(3, 3));
        yield return GetArbitraryBoardCase(new Vector2Int(1, 1), new Vector2Int(2, 2), new Vector2Int(3, 3), new Vector2Int(4, 4));
        yield return GetArbitraryBoardCase(new Vector2Int(1, 0), new Vector2Int(2, 3), new Vector2Int(5, 3), new Vector2Int(4, 4), new Vector2Int(5, 5));
    }

    private static object[] GetFullBoardCase(int boardLength, int boardWidth)
    {
        var board = GetFullBoard(boardLength, boardWidth);
        var expectedMatrix = new bool[boardLength, boardWidth];
        for (int i = 0; i < expectedMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < expectedMatrix.GetLength(1); j++)
            {
                expectedMatrix[i, j] = true;
            }
        }

        return new object[]
        {
            board,
            expectedMatrix
        };
    }

    private static object[] GetArbitraryBoardCase(params Vector2Int[] cellsPositions)
    {
        var board = new Board(CellType.Square);
        foreach (var pos in cellsPositions)
        {
            board.AddCell(pos);
        }

        var expectedMatrix = new bool[board.GetLength(0), board.GetLength(1)];
        foreach (var pos in cellsPositions)
        {
            expectedMatrix[pos.X, pos.Y] = true;
        }
        
        return new object[]
        {
            board,
            expectedMatrix
        };
    }
}