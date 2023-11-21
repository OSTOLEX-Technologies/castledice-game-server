using castledice_game_data_logic.Content;
using castledice_game_logic;
using static castledice_game_server_tests.ObjectCreationUtility;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests;

public class BoardDataProviderTests
{
    [Theory]
    [MemberData(nameof(BoardDataSizeTestCases))]
    public void GetBoardData_ShouldReturnBoardDataWithCorrectBoardSize_AccordingToGivenBoard(Board board)
    {
        var cellsPresenceProvider = new Mock<ICellsPresenceMatrixProvider>().Object;
        var contentDataListProvider = new Mock<IContentDataListProvider>().Object;
        var boardDataProvider = new BoardDataProvider(cellsPresenceProvider, contentDataListProvider);
        
        var boardData = boardDataProvider.GetBoardData(board);
        
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
        var cellsPresenceProvider = new Mock<ICellsPresenceMatrixProvider>().Object;
        var contentDataListProvider = new Mock<IContentDataListProvider>().Object;
        var boardDataProvider = new BoardDataProvider(cellsPresenceProvider, contentDataListProvider);
        
        var boardData = boardDataProvider.GetBoardData(board);
        
        Assert.Equal(cellType, boardData.CellType);
    }

    [Fact]
    public void GetBoardData_ShouldUseGivenCellsPresenceMatrixProvider_OnlyOnce()
    {
        var cellsPresenceProvider = new Mock<ICellsPresenceMatrixProvider>();
        var contentDataListProvider = new Mock<IContentDataListProvider>().Object;
        var boardDataProvider = new BoardDataProvider(cellsPresenceProvider.Object, contentDataListProvider);
        var board = GetFullBoard(10, 10);
        
        boardDataProvider.GetBoardData(board);

        cellsPresenceProvider.Verify(provider => provider.GetCellsPresenceMatrix(It.IsAny<Board>()), Times.Once);
    }

    [Fact]
    public void GetBoardData_ShouldPassGivenBoard_ToGivenCellsPresenceMatrixProvider()
    {
        var board = new Board(CellType.Square);
        var cellsPresenceProvider = new Mock<ICellsPresenceMatrixProvider>();
        var contentDataListProvider = new Mock<IContentDataListProvider>().Object;
        var boardDataProvider = new BoardDataProvider(cellsPresenceProvider.Object, contentDataListProvider);
        
        boardDataProvider.GetBoardData(board);
        
        cellsPresenceProvider.Verify(provider => provider.GetCellsPresenceMatrix(board), Times.Once);
    }

    [Fact]
    public void GetBoardData_ShouldReturnBoardDataWithCellsPresenceMatrix_FromGivenProvider()
    {
        var expectedMatrix = new bool[1,1];
        var cellsPresenceProvider = new Mock<ICellsPresenceMatrixProvider>();
        cellsPresenceProvider.Setup(provider => provider.GetCellsPresenceMatrix(It.IsAny<Board>()))
            .Returns(expectedMatrix);
        var contentDataListProvider = new Mock<IContentDataListProvider>().Object;
        var boardDataProvider = new BoardDataProvider(cellsPresenceProvider.Object, contentDataListProvider);
        
        var boardData = boardDataProvider.GetBoardData(GetFullBoard(10, 10));
        
        Assert.Same(expectedMatrix, boardData.CellsPresence);
    }

    [Fact]
    public void GetBoardData_ShouldUseGivenContentDataListProvider_OnlyOnce()
    {
        var cellsPresenceProvider = new Mock<ICellsPresenceMatrixProvider>().Object;
        var contentDataListProvider = new Mock<IContentDataListProvider>();
        var boardDataProvider = new BoardDataProvider(cellsPresenceProvider, contentDataListProvider.Object);
        
        boardDataProvider.GetBoardData(GetFullBoard(10, 10));
        
        contentDataListProvider.Verify(provider => provider.GetContentDataList(It.IsAny<Board>()), Times.Once);
    }
    
    [Fact]
    public void GetBoardData_ShouldPassGivenBoard_ToGivenContentDataListProvider()
    {
        var board = new Board(CellType.Square);
        var cellsPresenceProvider = new Mock<ICellsPresenceMatrixProvider>().Object;
        var contentDataListProvider = new Mock<IContentDataListProvider>();
        var boardDataProvider = new BoardDataProvider(cellsPresenceProvider, contentDataListProvider.Object);
        
        boardDataProvider.GetBoardData(board);
        
        contentDataListProvider.Verify(provider => provider.GetContentDataList(board), Times.Once);
    }

    [Fact]
    public void GetBoardData_ShouldReturnBoardDataWithContentDataList_FromGivenProvider()
    {
        var expectedList = new List<ContentData>();
        var cellsPresenceProvider = new Mock<ICellsPresenceMatrixProvider>().Object;
        var contentDataListProvider = new Mock<IContentDataListProvider>();
        contentDataListProvider.Setup(provider => provider.GetContentDataList(It.IsAny<Board>()))
            .Returns(expectedList);
        var boardDataProvider = new BoardDataProvider(cellsPresenceProvider, contentDataListProvider.Object);
        
        var boardData = boardDataProvider.GetBoardData(GetFullBoard(10, 10));
        
        Assert.Same(expectedList, boardData.GeneratedContent);
    }
}