using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.CellsGeneratorsProviders;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests.CellsGeneratiorProvidersTests;

/// <summary>
/// Due to git inability to index file name "DefaultRectGenerationConfigProviderTests" the name of this file was shortened in a following way.
/// </summary>
public class DefRectGenConfigProviderTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 20)]
    [InlineData(15, 12)]
    [InlineData(100, 100)]
    public void GetRectGenerationConfig_ShouldReturnConfig_WithValuesFromProviderConstructor(int boardLength,
        int boardWidth)
    {
        var configProvider = new DefaultRectGenerationConfigProvider(boardWidth, boardLength);
        
        var config = configProvider.GetRectGenerationConfig();
        
        Assert.Equal(boardLength, config.BoardLength);
        Assert.Equal(boardWidth, config.BoardWidth);
    }
}