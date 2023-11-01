using castledice_game_logic;
using castledice_game_server.Exceptions;
using castledice_game_server.GameController;
using castledice_game_server.GameController.Moves;
using static castledice_game_server_tests.ObjectCreationUtility;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.MovesControllerTests;

public class GameForPlayerProviderTests
{
    [Fact]
    public void GetGame_ShouldThrowGameNotFoundException_IfGameWithGivenPlayerIdDoesNotExists()
    {
        var gamesCollection = new Mock<IGamesCollection>().Object;
        var gameForPlayerProvider = new GameForPlayerProvider(gamesCollection);
        
        Assert.Throws<GameNotFoundException>(() => gameForPlayerProvider.GetGame(1));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetGame_ShouldReturnGameWithGivenPlayerId_IfGameWithGivenPlayerIdExists(int playerId)
    {
        var game = GetGame(GetPlayer(playerId), GetPlayer(playerId + 1));
        var gamesCollectionMock = new Mock<IGamesCollection>();
        var gamesList = new List<Game>() { game };
        gamesCollectionMock.Setup(g => g.GetEnumerator()).Returns(gamesList.GetEnumerator());
        var gameForPlayerProvider = new GameForPlayerProvider(gamesCollectionMock.Object);
        
        var result = gameForPlayerProvider.GetGame(playerId);
        
        Assert.Same(game, result);
    }
}