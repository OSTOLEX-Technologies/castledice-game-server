using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.CellsGeneratorsCreators;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.CellsGenerationCreatorsTests;

public class RectGenerationCreatorTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 2)]
    public void Properties_ShouldReturnValues_GivenInConstructor(int boardWidth, int boardLength)
    {
        var config = new RectGenerationConfig(boardWidth, boardLength);
        
        Assert.Equal(boardWidth, config.BoardWidth);
        Assert.Equal(boardLength, config.BoardLength);
    }
}