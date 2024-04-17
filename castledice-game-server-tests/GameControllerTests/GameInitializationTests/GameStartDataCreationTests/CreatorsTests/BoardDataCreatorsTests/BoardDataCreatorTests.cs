using castledice_game_data_logic.Content;
using castledice_game_logic;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.BoardDataCreators;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.BoardDataCreators.ContentDataCreators;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests.CreatorsTests.BoardDataCreatorsTests;

public class BoardDataCreatorTests
{
    [Theory]
    [MemberData(nameof(BoardDataSizeTestCases))]
    public void GetBoardData_ShouldReturnBoardDataWithCorrectBoardSize_AccordingToGivenBoard(Board board)
    {
        var cellsPresenceCreator = new Mock<ICellsPresenceMatrixCreator>().Object;
        var contentDataListCreator = new Mock<IContentDataListCreator>().Object;
        var boardDataCreator = new BoardDataCreator(cellsPresenceCreator, contentDataListCreator);
        
        var boardData = boardDataCreator.GetBoardData(board);
        
        Assert.Equal(board.GetLength(0), boardData.BoardLength);
        Assert.Equal(board.GetLength(1), boardData.BoardWidth);
    }

    public static IEnumerable<object[]> BoardDataSizeTestCases()
    {
        yield return new[]
        {
            GetFullBoard(10 , 10)
        };
        yield return new[]
        {
            GetFullBoard(5 , 5)
        };
        yield return new[]
        {
            GetFullBoard(1 , 1)
        };
    }

    [Theory]
    [InlineData(CellType.Square)]
    [InlineData(CellType.Triangle)]
    public void GetBoardData_ShouldReturnBoardDataWithCellType_EqualToGivenBoardCellType(CellType cellType)
    {
        var board = new Board(cellType);
        var cellsPresenceCreator = new Mock<ICellsPresenceMatrixCreator>().Object;
        var contentDataListCreator = new Mock<IContentDataListCreator>().Object;
        var boardDataCreator = new BoardDataCreator(cellsPresenceCreator, contentDataListCreator);
        
        var boardData = boardDataCreator.GetBoardData(board);
        
        Assert.Equal(cellType, boardData.CellType);
    }

    [Fact]
    public void GetBoardData_ShouldUseGivenCellsPresenceMatrixCreator_OnlyOnce()
    {
        var cellsPresenceCreator = new Mock<ICellsPresenceMatrixCreator>();
        var contentDataListCreator = new Mock<IContentDataListCreator>().Object;
        var boardDataCreator = new BoardDataCreator(cellsPresenceCreator.Object, contentDataListCreator);
        var board = GetFullBoard(10, 10);
        
        boardDataCreator.GetBoardData(board);

        cellsPresenceCreator.Verify(creator => creator.GetCellsPresenceMatrix(It.IsAny<Board>()), Times.Once);
    }

    [Fact]
    public void GetBoardData_ShouldPassGivenBoard_ToGivenCellsPresenceMatrixCreator()
    {
        var board = new Board(CellType.Square);
        var cellsPresenceCreator = new Mock<ICellsPresenceMatrixCreator>();
        var contentDataListCreator = new Mock<IContentDataListCreator>().Object;
        var boardDataCreator = new BoardDataCreator(cellsPresenceCreator.Object, contentDataListCreator);
        
        boardDataCreator.GetBoardData(board);
        
        cellsPresenceCreator.Verify(creator => creator.GetCellsPresenceMatrix(board), Times.Once);
    }

    [Fact]
    public void GetBoardData_ShouldReturnBoardDataWithCellsPresenceMatrix_FromGivenCreator()
    {
        var expectedMatrix = new bool[1,1];
        var cellsPresenceCreator = new Mock<ICellsPresenceMatrixCreator>();
        cellsPresenceCreator.Setup(creator => creator.GetCellsPresenceMatrix(It.IsAny<Board>()))
            .Returns(expectedMatrix);
        var contentDataListCreator = new Mock<IContentDataListCreator>().Object;
        var boardDataCreator = new BoardDataCreator(cellsPresenceCreator.Object, contentDataListCreator);
        
        var boardData = boardDataCreator.GetBoardData(GetFullBoard(10, 10));
        
        Assert.Same(expectedMatrix, boardData.CellsPresence);
    }

    [Fact]
    public void GetBoardData_ShouldUseGivenContentDataListCreator_OnlyOnce()
    {
        var cellsPresenceCreator = new Mock<ICellsPresenceMatrixCreator>().Object;
        var contentDataListCreator = new Mock<IContentDataListCreator>();
        var boardDataCreator = new BoardDataCreator(cellsPresenceCreator, contentDataListCreator.Object);
        
        boardDataCreator.GetBoardData(GetFullBoard(10, 10));
        
        contentDataListCreator.Verify(creator => creator.GetContentDataList(It.IsAny<Board>()), Times.Once);
    }
    
    [Fact]
    public void GetBoardData_ShouldPassGivenBoard_ToGivenContentDataListCreator()
    {
        var board = new Board(CellType.Square);
        var cellsPresenceCreator = new Mock<ICellsPresenceMatrixCreator>().Object;
        var contentDataListCreator = new Mock<IContentDataListCreator>();
        var boardDataCreator = new BoardDataCreator(cellsPresenceCreator, contentDataListCreator.Object);
        
        boardDataCreator.GetBoardData(board);
        
        contentDataListCreator.Verify(creator => creator.GetContentDataList(board), Times.Once);
    }

    [Fact]
    public void GetBoardData_ShouldReturnBoardDataWithContentDataList_FromGivenCreator()
    {
        var expectedList = new List<ContentData>();
        var cellsPresenceCreator = new Mock<ICellsPresenceMatrixCreator>().Object;
        var contentDataListCreator = new Mock<IContentDataListCreator>();
        contentDataListCreator.Setup(creator => creator.GetContentDataList(It.IsAny<Board>()))
            .Returns(expectedList);
        var boardDataCreator = new BoardDataCreator(cellsPresenceCreator, contentDataListCreator.Object);
        
        var boardData = boardDataCreator.GetBoardData(GetFullBoard(10, 10));
        
        Assert.Same(expectedList, boardData.GeneratedContent);
    }
}