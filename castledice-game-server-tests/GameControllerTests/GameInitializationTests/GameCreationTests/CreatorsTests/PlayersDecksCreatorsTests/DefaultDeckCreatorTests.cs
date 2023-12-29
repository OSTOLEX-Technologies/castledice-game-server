using castledice_game_logic.GameObjects;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersDecksCreators;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.PlayersDecksCreatorsTests;

public class DefaultDeckCreatorTests
{
    [Fact]
    public void GetDeckForPlayer_ShouldReturnDefaultDeck_GivenInConstructor()
    {
        var playerId = GetRandomId();
        var expectedDeck = new List<PlacementType>();
        var deckCreator = new DefaultDeckCreator(expectedDeck);
        
        var deck = deckCreator.GetDeckForPlayer(playerId);
        
        Assert.Same(expectedDeck, deck);
    }

    private static int GetRandomId()
    {
        var rnd = new Random();
        return rnd.Next();
    }
}