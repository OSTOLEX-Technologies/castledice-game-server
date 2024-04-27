using castledice_game_logic;
using castledice_game_server.Exceptions;
using castledice_game_server.GameController;
using castledice_game_server.GameController.General;
using castledice_game_server.GameController.Moves;
using static castledice_game_server_tests.ObjectCreationUtility;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.MovesControllerTests;

public class GameForPlayerProviderTests
{
    [Fact]
    public void GetGame_ShouldThrowGameNotFoundException_IfGameWithGivenPlayerIdDoesNotExists()
    {
        var gamesCollectionMock = new Mock<IGamesCollection>();
        var gamesList = new List<Game>() {  };
        gamesCollectionMock.Setup(g => g.GetEnumerator()).Returns(gamesList.GetEnumerator());
        var gameForPlayerProvider = new GameForPlayerProvider(gamesCollectionMock.Object);
        
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
    
    [Fact]
    public void PlayerIsInGame_ShouldReturnFalse_IfPlayerIsNotInAnyGame()
    {
        var gamesCollectionMock = new Mock<IGamesCollection>();
        var playerId = new Random().Next(3, 10000);
        var gamesList = new List<Game>() { GetGame(GetPlayer(0), GetPlayer(2)) };
        gamesCollectionMock.Setup(g => g.GetEnumerator()).Returns(gamesList.GetEnumerator());
        var gameForPlayerProvider = new GameForPlayerProvider(gamesCollectionMock.Object);
        
        var result = gameForPlayerProvider.PlayerIsInGame(playerId);
        
        Assert.False(result);
    }

    [Fact]
    public void PlayerIsInGame_ShouldReturnTrue_IfThereIsGameWithPlayer_WithGivenId()
    {
        var playerId = new Random().Next();
        var game = GetGame(GetPlayer(1), GetPlayer(playerId));
        var gamesCollectionMock = new Mock<IGamesCollection>();
        var gamesList = new List<Game>() { game };
        gamesCollectionMock.Setup(g => g.GetEnumerator()).Returns(gamesList.GetEnumerator());
        var gameForPlayerProvider = new GameForPlayerProvider(gamesCollectionMock.Object);
        
        var result = gameForPlayerProvider.PlayerIsInGame(playerId);
        
        Assert.True(result);
    }
}