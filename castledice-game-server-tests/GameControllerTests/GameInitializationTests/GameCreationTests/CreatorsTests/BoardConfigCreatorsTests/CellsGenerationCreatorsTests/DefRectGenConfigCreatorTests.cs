using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.CellsGeneratorsCreators;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.CellsGenerationCreatorsTests;

/// <summary>
/// Due to git inability to index file name "DefaultRectGenerationConfigCreatorTests" the name of this file was shortened in a following way.
/// </summary>
public class DefRectGenConfigCreatorTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 20)]
    [InlineData(15, 12)]
    [InlineData(100, 100)]
    public void GetRectGenerationConfig_ShouldReturnConfig_WithValuesFromCreatorConstructor(int boardLength,
        int boardWidth)
    {
        var configCreator = new DefaultRectGenerationConfigCreator(boardWidth, boardLength);
        
        var config = configCreator.GetRectGenerationConfig();
        
        Assert.Equal(boardLength, config.BoardLength);
        Assert.Equal(boardWidth, config.BoardWidth);
    }
}