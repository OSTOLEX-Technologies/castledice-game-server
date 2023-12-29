using castledice_game_logic.BoardGeneration.CellsGeneration;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.CellsGeneratorsCreators;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.CellsGenerationCreatorsTests;

public class RectCellsGeneratorCreatorTests
{
    [Fact]
    public void GetCellsGenerator_ShouldReturnRectCellsGenerator()
    {
        var cellsGeneratorCreator = new RectCellsGeneratorCreator(GetRectGenerationConfigCreatorMock().Object);
        
        var cellsGenerator = cellsGeneratorCreator.GetCellsGenerator();
        
        Assert.IsType<RectCellsGenerator>(cellsGenerator);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 20)]
    [InlineData(15, 12)]
    [InlineData(100, 100)]
    public void ReturnedRectCellsGenerator_ShouldHavePropertiesCorrespondingToConfig_FromGivenConfigCreator(
        int boardWidth, int boardLength)
    {
        var configToReturn = new RectGenerationConfig(boardWidth, boardLength);
        var rectGenerationConfigCreatorMock = GetRectGenerationConfigCreatorMock();
        rectGenerationConfigCreatorMock.Setup(x => x.GetRectGenerationConfig()).Returns(configToReturn);
        var cellsGeneratorCreator = new RectCellsGeneratorCreator(rectGenerationConfigCreatorMock.Object);
        
        var cellsGenerator = cellsGeneratorCreator.GetCellsGenerator();
        var rectCellsGenerator = cellsGenerator as RectCellsGenerator;
        
        Assert.Equal(boardWidth, rectCellsGenerator.BoardWidth);
        Assert.Equal(boardLength, rectCellsGenerator.BoardLength);
    }
    
    private static Mock<IRectGenerationConfigCreator> GetRectGenerationConfigCreatorMock()
    {
        var rectGenerationConfigCreatorMock = new Mock<IRectGenerationConfigCreator>();
        rectGenerationConfigCreatorMock.Setup(x => x.GetRectGenerationConfig()).Returns(new RectGenerationConfig(1, 1));
        return rectGenerationConfigCreatorMock;
    }
}