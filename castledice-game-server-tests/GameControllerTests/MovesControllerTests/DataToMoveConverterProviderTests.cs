using castledice_game_data_logic.MoveConverters;
using castledice_game_server.GameController.Moves;
using static castledice_game_server_tests.ObjectCreationUtility;

namespace castledice_game_server_tests.GameControllerTests.MovesControllerTests;

public class DataToMoveConverterProviderTests
{
    [Fact]
    public void GetDataToMove_ShouldReturnDataToMoveConverter_WithPlaceablesFactoryFromGame()
    {
        var game = GetGame();
        var expectedFactory = game.PlaceablesFactory;
        var dataToMoveConverterProvider = new DataToMoveConverterProvider();
        
        var result = dataToMoveConverterProvider.GetDataToMoveConverter(game);
        var converter = result as DataToMoveConverter;
        
        Assert.Same(expectedFactory, converter.Factory);
    }
}