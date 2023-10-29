using castledice_game_logic.GameObjects;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersDecksListsProviders;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.PlayersDecksListsProvidersTests;

public class DefaultDeckProviderTests
{
    [Fact]
    public void GetDeckForPlayer_ShouldReturnDefaultDeck_GivenInConstructor()
    {
        var playerId = GetRandomId();
        var expectedDeck = new List<PlacementType>();
        var deckProvider = new DefaultDeckProvider(expectedDeck);
        
        var deck = deckProvider.GetDeckForPlayer(playerId);
        
        Assert.Same(expectedDeck, deck);
    }

    private static int GetRandomId()
    {
        var rnd = new Random();
        return rnd.Next();
    }
}