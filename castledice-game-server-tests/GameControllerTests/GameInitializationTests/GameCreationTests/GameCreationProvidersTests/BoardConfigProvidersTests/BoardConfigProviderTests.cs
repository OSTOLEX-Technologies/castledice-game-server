using castledice_game_logic;
using castledice_game_logic.BoardGeneration.CellsGeneration;
using castledice_game_logic.BoardGeneration.ContentGeneration;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests;

public class BoardConfigProviderTests
{
    private readonly Mock<ICellsGeneratorProvider> _cellsGeneratorProviderMock = new();
    private readonly Mock<IContentSpawnersListProvider> _contentSpawnersListProviderMock = new();
    
    [Fact]
    public void GetBoardConfig_ShouldReturnBoardConfig_WithSquareCellType()
    {
        var boardConfigProvider = new BoardConfigProvider(_cellsGeneratorProviderMock.Object, _contentSpawnersListProviderMock.Object);
        
        var boardConfig = boardConfigProvider.GetBoardConfig(new List<Player>());
        
        Assert.Equal(CellType.Square, boardConfig.CellType);
    }

    [Fact]
    public void GetBoardConfig_ShouldReturnBoardConfig_WithSpawnersListFromSpawnersProvider()
    {
        var expectedList = new List<IContentSpawner>();
        _contentSpawnersListProviderMock.Setup(x => x.GetContentSpawners(It.IsAny<List<Player>>())).Returns(expectedList);
        var boardConfigProvider = new BoardConfigProvider(_cellsGeneratorProviderMock.Object, _contentSpawnersListProviderMock.Object);
        
        var boardConfig = boardConfigProvider.GetBoardConfig(new List<Player>());
        
        Assert.Same(expectedList, boardConfig.ContentSpawners);
    }
    
    [Fact]
    public void GetBoardConfig_ShouldReturnBoardConfig_WithCellsGeneratorFromGeneratorProvider()
    {
        var expectedGenerator = new Mock<ICellsGenerator>().Object;
        _cellsGeneratorProviderMock.Setup(x => x.GetCellsGenerator()).Returns(expectedGenerator);
        var boardConfigProvider = new BoardConfigProvider(_cellsGeneratorProviderMock.Object, _contentSpawnersListProviderMock.Object);
        
        var boardConfig = boardConfigProvider.GetBoardConfig(new List<Player>());
        
        Assert.Same(expectedGenerator, boardConfig.CellsGenerator);
    }
}