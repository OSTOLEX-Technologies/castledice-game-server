using castledice_game_logic;
using castledice_game_logic.BoardGeneration.CellsGeneration;
using castledice_game_logic.BoardGeneration.ContentGeneration;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.CellsGeneratorsCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests;

public class BoardConfigCreatorTests
{
    private readonly Mock<ICellsGeneratorCreator> _cellsGeneratorCreatorMock = new();
    private readonly Mock<IContentSpawnersListCreator> _contentSpawnersListCreatorMock = new();
    
    [Fact]
    public void GetBoardConfig_ShouldReturnBoardConfig_WithSquareCellType()
    {
        var boardConfigCreator = new BoardConfigCreator(_cellsGeneratorCreatorMock.Object, _contentSpawnersListCreatorMock.Object);
        
        var boardConfig = boardConfigCreator.GetBoardConfig(new List<Player>());
        
        Assert.Equal(CellType.Square, boardConfig.CellType);
    }

    [Fact]
    public void GetBoardConfig_ShouldReturnBoardConfig_WithSpawnersListFromSpawnersCreator()
    {
        var expectedList = new List<IContentSpawner>();
        _contentSpawnersListCreatorMock.Setup(x => x.GetContentSpawners(It.IsAny<List<Player>>())).Returns(expectedList);
        var boardConfigCreator = new BoardConfigCreator(_cellsGeneratorCreatorMock.Object, _contentSpawnersListCreatorMock.Object);
        
        var boardConfig = boardConfigCreator.GetBoardConfig(new List<Player>());
        
        Assert.Same(expectedList, boardConfig.ContentSpawners);
    }

    [Fact]
    public void GetBoardConfig_ShouldPassGivenPlayersList_ToGivenContentSpawnersCreator()
    {
        var playersList = new List<Player>();
        var boardConfigCreator = new BoardConfigCreator(_cellsGeneratorCreatorMock.Object, _contentSpawnersListCreatorMock.Object);
        
        boardConfigCreator.GetBoardConfig(playersList);
        
        _contentSpawnersListCreatorMock.Verify(x => x.GetContentSpawners(playersList), Times.Once);
    }
    
    [Fact]
    public void GetBoardConfig_ShouldReturnBoardConfig_WithCellsGeneratorFromGeneratorCreator()
    {
        var expectedGenerator = new Mock<ICellsGenerator>().Object;
        _cellsGeneratorCreatorMock.Setup(x => x.GetCellsGenerator()).Returns(expectedGenerator);
        var boardConfigCreator = new BoardConfigCreator(_cellsGeneratorCreatorMock.Object, _contentSpawnersListCreatorMock.Object);
        
        var boardConfig = boardConfigCreator.GetBoardConfig(new List<Player>());
        
        Assert.Same(expectedGenerator, boardConfig.CellsGenerator);
    }
}