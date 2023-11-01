using castledice_game_logic;
using castledice_game_server.GameController;
using static castledice_game_server_tests.ObjectCreationUtility;

namespace castledice_game_server_tests.GameControllerTests;

public class ActiveGamesCollectionTests
{
    [Fact]
    public void AddGame_ShouldThrowArgumentException_IfGameWithGivenIdAlreadyExists()
    {
        var gamesCollection = new ActiveGamesCollection();
        var game = GetGame();
        gamesCollection.AddGame(1, game);
        
        Assert.Throws<ArgumentException>(() => gamesCollection.AddGame(1, GetGame()));
    }

    [Fact]
    public void AddGame_ShouldThrowArgumentException_IfGivenGameInstanceAlreadyExists()
    {
        var gamesCollection = new ActiveGamesCollection();
        var game = GetGame();
        gamesCollection.AddGame(1, game);
        
        Assert.Throws<ArgumentException>(() => gamesCollection.AddGame(2, game));
    }

    [Fact]
    public void AddGame_ShouldAddGivenGame_ToCollection()
    {
        var gamesCollection = new ActiveGamesCollection();
        var game = GetGame();
        gamesCollection.AddGame(1, game);

        Assert.Contains(game, gamesCollection);
    }
    
    [Fact]
    public void AddGame_ShouldInvokeGameAddedEvent_AndPassAddedGameAsAnArgument()
    {
        var gamesCollection = new ActiveGamesCollection();
        var game = GetGame();
        Game? addedGame = null;
        gamesCollection.GameAdded += (_, game) => addedGame = game;
        
        gamesCollection.AddGame(1, game);
        
        Assert.Same(game, addedGame);
    }

    [Fact]
    public void GetGameId_ShouldThrowArgumentException_IfGivenGameInstanceDoesNotExists()
    {
        var gamesCollection = new ActiveGamesCollection();
        var game = GetGame();
        
        Assert.Throws<ArgumentException>(() => gamesCollection.GetGameId(game));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetGameId_ShouldReturnIdOfAddedGame(int expectedId)
    {
        var gamesCollection = new ActiveGamesCollection();
        var game = GetGame();
        gamesCollection.AddGame(expectedId, game);
        
        var actualId = gamesCollection.GetGameId(game);
        
        Assert.Equal(expectedId, actualId);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void RemoveGame_ShouldReturnFalse_IfGameWithGivenIdDoesNotExists(int id)
    {
        var gamesCollection = new ActiveGamesCollection();
        
        var result = gamesCollection.RemoveGame(1);
        
        Assert.False(result);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void RemoveGame_ShouldReturnTrue_IfGameWithGivenIdExists(int id)
    {
        var gamesCollection = new ActiveGamesCollection();
        var game = GetGame();
        gamesCollection.AddGame(id, game);
        
        var result = gamesCollection.RemoveGame(id);
        
        Assert.True(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void RemoveGame_ShouldRemoveGameWithGivenId_FromCollection(int id)
    {
        var gamesCollection = new ActiveGamesCollection();
        var game = GetGame();
        gamesCollection.AddGame(id, game);
        
        gamesCollection.RemoveGame(id);
        
        Assert.DoesNotContain(game, gamesCollection);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void RemoveGame_ShouldInvokeGameRemovedEvent_AndPassRemovedGameAsAnArgument(int id)
    {
        var gamesCollection = new ActiveGamesCollection();
        var game = GetGame();
        gamesCollection.AddGame(id, game);
        Game? removedGame = null;
        gamesCollection.GameRemoved += (_, game) => removedGame = game;
        
        gamesCollection.RemoveGame(id);
        
        Assert.Same(game, removedGame);
    }
}