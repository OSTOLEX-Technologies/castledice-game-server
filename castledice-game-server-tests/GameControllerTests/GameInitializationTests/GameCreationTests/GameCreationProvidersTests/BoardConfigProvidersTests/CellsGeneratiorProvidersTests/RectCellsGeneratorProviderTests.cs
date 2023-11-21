using castledice_game_logic.BoardGeneration.CellsGeneration;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.CellsGeneratorsProviders;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests.CellsGeneratiorProvidersTests;

public class RectCellsGeneratorProviderTests
{
    [Fact]
    public void GetCellsGenerator_ShouldReturnRectCellsGenerator()
    {
        var cellsGeneratorProvider = new RectCellsGeneratorProvider(GetRectGenerationConfigProviderMock().Object);
        
        var cellsGenerator = cellsGeneratorProvider.GetCellsGenerator();
        
        Assert.IsType<RectCellsGenerator>(cellsGenerator);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 20)]
    [InlineData(15, 12)]
    [InlineData(100, 100)]
    public void ReturnedRectCellsGenerator_ShouldHavePropertiesCorrespondingToConfig_FromGivenConfigProvider(
        int boardWidth, int boardLength)
    {
        var configToReturn = new RectGenerationConfig(boardWidth, boardLength);
        var rectGenerationConfigProviderMock = GetRectGenerationConfigProviderMock();
        rectGenerationConfigProviderMock.Setup(x => x.GetRectGenerationConfig()).Returns(configToReturn);
        var cellsGeneratorProvider = new RectCellsGeneratorProvider(rectGenerationConfigProviderMock.Object);
        
        var cellsGenerator = cellsGeneratorProvider.GetCellsGenerator();
        var rectCellsGenerator = cellsGenerator as RectCellsGenerator;
        
        Assert.Equal(boardWidth, rectCellsGenerator.BoardWidth);
        Assert.Equal(boardLength, rectCellsGenerator.BoardLength);
    }
    
    private static Mock<IRectGenerationConfigProvider> GetRectGenerationConfigProviderMock()
    {
        var rectGenerationConfigProviderMock = new Mock<IRectGenerationConfigProvider>();
        rectGenerationConfigProviderMock.Setup(x => x.GetRectGenerationConfig()).Returns(new RectGenerationConfig(1, 1));
        return rectGenerationConfigProviderMock;
    }
}